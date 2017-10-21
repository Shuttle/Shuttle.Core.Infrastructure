using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Shuttle.Core.Infrastructure
{
	public class ReflectionService : IReflectionService
	{
        private readonly List<string> _assemblyExtensions = new List<string>
        {
            ".dll",
            ".exe"
        };

		private readonly ILog _log;

		public ReflectionService()
		{
			_log = Log.For(this);
		}

		public string AssemblyPath(Assembly assembly)
		{
			Guard.AgainstNull(assembly, "assembly");

			return !assembly.IsDynamic
				? new Uri(Uri.UnescapeDataString(new UriBuilder(assembly.CodeBase).Path)).LocalPath
				: string.Empty;
		}

		public Assembly GetAssembly(string assemblyPath)
		{
            var result = AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(assembly => AssemblyPath(assembly)
                    .Equals(assemblyPath, StringComparison.InvariantCultureIgnoreCase));

			if (result != null)
			{
				return result;
			}

			try
			{
				switch (Path.GetExtension(assemblyPath))
				{
					case ".dll":
					case ".exe":
						{
							result = Path.GetDirectoryName(assemblyPath) == AppDomain.CurrentDomain.BaseDirectory
								? Assembly.Load(Path.GetFileNameWithoutExtension(assemblyPath))
								: Assembly.LoadFile(assemblyPath);
							break;
						}

					default:
						{
							result = Assembly.Load(assemblyPath);

							break;
						}
				}
			}
			catch (Exception ex)
			{
				_log.Warning(string.Format(InfrastructureResources.AssemblyLoadException, assemblyPath, ex.Message));

				var reflection = ex as ReflectionTypeLoadException;

				if (reflection != null)
				{
					foreach (var exception in reflection.LoaderExceptions)
					{
                        _log.Trace($"'{exception.AllMessages()}'.");
					}
				}
				else
				{
                    _log.Trace($"{ex.GetType()}: '{ex.AllMessages()}'.");
				}

				return null;
			}

			return result;
        }

        public Assembly FindAssemblyNamed(string name)
        {
            Guard.AgainstNullOrEmptyString(name, nameof(name));

            var assemblyName = name;
            var hasFileExtension = false;

            if (name.EndsWith(".dll", StringComparison.CurrentCultureIgnoreCase)
                ||
                name.EndsWith(".exe", StringComparison.CurrentCultureIgnoreCase))
            {
                assemblyName = Path.GetFileNameWithoutExtension(name);
                hasFileExtension = true;
            }

            var result = AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(assembly => assembly.GetName()
                    .Name.Equals(assemblyName, StringComparison.InvariantCultureIgnoreCase));

            if (result != null)
            {
                return result;
            }

            var privateBinPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                AppDomain.CurrentDomain.RelativeSearchPath ?? string.Empty);

            var extensions = new List<string>();

            if (hasFileExtension)
            {
                extensions.Add(string.Empty);
            }
            else
            {
                extensions.AddRange(_assemblyExtensions);
            }

            foreach (var extension in extensions)
            {
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Concat(name, extension));

                if (File.Exists(path))
                {
                    return GetAssembly(path);
                }

                if (!privateBinPath.Equals(AppDomain.CurrentDomain.BaseDirectory))
                {
                    path = Path.Combine(privateBinPath, string.Concat(name, extension));

                    if (File.Exists(path))
                    {
                        return GetAssembly(path);
                    }
                }
            }

            return null;
        }

        public IEnumerable<Assembly> GetAssemblies(string folder)
        {
            return GetMatchingAssemblies(string.Empty, folder);
        }

        public IEnumerable<Assembly> GetAssemblies()
        {
            return GetMatchingAssemblies(string.Empty);
        }

        public IEnumerable<Assembly> GetMatchingAssemblies(string regex, string folder)
        {
            var expression = new Regex(regex, RegexOptions.IgnoreCase);
            var result = new List<Assembly>();

            if (Directory.Exists(folder))
            {
                result.AddRange(
                    Directory.GetFiles(folder, "*.exe")
                        .Where(file => expression.IsMatch(Path.GetFileNameWithoutExtension(file)))
                        .Select(GetAssembly)
                        .Where(assembly => assembly != null));
                result.AddRange(
                    Directory.GetFiles(folder, "*.dll")
                        .Where(file => expression.IsMatch(Path.GetFileNameWithoutExtension(file)))
                        .Select(GetAssembly)
                        .Where(assembly => assembly != null));
			}

			return result;
		}

        public IEnumerable<Assembly> GetMatchingAssemblies(string regex)
        {
            var assemblies = new List<Assembly>(AppDomain.CurrentDomain.GetAssemblies());

			foreach (
				var assembly in
                GetMatchingAssemblies(regex, AppDomain.CurrentDomain.BaseDirectory)
						.Where(assembly => assemblies.Find(candidate => candidate.Equals(assembly)) == null))
			{
				assemblies.Add(assembly);
			}

			var privateBinPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
				AppDomain.CurrentDomain.RelativeSearchPath ?? string.Empty);

			if (!privateBinPath.Equals(AppDomain.CurrentDomain.BaseDirectory))
			{
				foreach (
					var assembly in
                    GetMatchingAssemblies(regex, privateBinPath)
							.Where(assembly => assemblies.Find(candidate => candidate.Equals(assembly)) == null))
				{
					assemblies.Add(assembly);
				}
			}

			return assemblies;
		}

		public IEnumerable<Type> GetTypes<T>()
		{
			return GetTypes(typeof(T));
		}

		public IEnumerable<Type> GetTypes(Type type)
		{
			var result = new List<Type>();

			foreach (var assembly in GetAssemblies())
			{
				GetTypes(type, assembly)
					.Where(candidate => result.Find(existing => existing == candidate) == null)
					.ToList()
					.ForEach(add => result.Add(add));
			}

			return result;
		}

		public IEnumerable<Type> GetTypes<T>(Assembly assembly)
		{
			return GetTypes(typeof(T), assembly);
		}

		public IEnumerable<Type> GetTypes(Type type, Assembly assembly)
		{
			Guard.AgainstNull(type, "type");
			Guard.AgainstNull(assembly, "assemblyPath");

			return GetTypes(assembly).Where(candidate => candidate.IsAssignableTo(type) && candidate != type).ToList();
		}

		public IEnumerable<Type> GetTypes(Assembly assembly)
		{
			Type[] types;

			try
			{
				_log.Trace(string.Format(InfrastructureResources.TraceGetTypesFromAssembly, assembly));

				types = assembly.GetTypes();
			}
			catch (Exception ex)
			{
				var reflection = ex as ReflectionTypeLoadException;

				if (reflection != null)
				{
					foreach (var exception in reflection.LoaderExceptions)
					{
                        _log.Error($"'{exception.AllMessages()}'.");
					}
				}
				else
				{
                    _log.Error($"{ex.GetType()}: '{ex.AllMessages()}'.");
				}

				return new List<Type>();
			}

			return types;
		}
	}
}
