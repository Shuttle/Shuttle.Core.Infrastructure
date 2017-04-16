using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Shuttle.Core.Infrastructure
{
	public class ReflectionService : IReflectionService
	{
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
			var result = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(assembly => AssemblyPath(assembly).Equals(assemblyPath, StringComparison.InvariantCultureIgnoreCase));

			if (result != null)
			{
				return result;
			}

			try
			{
				switch ((Path.GetExtension(assemblyPath) ?? string.Empty).ToLowerInvariant())
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
						_log.Trace(string.Format("'{0}'.", exception.AllMessages()));
					}
				}
				else
				{
					_log.Trace(string.Format("{0}: '{1}'.", ex.GetType(), ex.AllMessages()));
				}

				return null;
			}

			return result;
		}

		public IEnumerable<Assembly> GetAssemblies(string folder)
		{
			var result = new List<Assembly>();

			if (Directory.Exists(folder))
			{
				result.AddRange(
					Directory.GetFiles(folder, "*.exe").Select(GetAssembly).Where(assembly => assembly != null));
				result.AddRange(
					Directory.GetFiles(folder, "*.dll").Select(GetAssembly).Where(assembly => assembly != null));
			}

			return result;
		}

        private IEnumerable<Assembly> GetAssembliesRecursive(string folder)
        {
            var result = new List<Assembly>(GetAssemblies(folder));

            foreach (var directory in Directory.GetDirectories(folder))
            {
                result.AddRange(GetAssembliesRecursive(directory));
            }

            return result;
        }

        public IEnumerable<Assembly> GetAssemblies()
		{
			var assemblies = new List<Assembly>(AppDomain.CurrentDomain.GetAssemblies());

            _log.Debug(string.Format(InfrastructureResources.DebugGetAssemblies, AppDomain.CurrentDomain.ShadowCopyFiles));

            if (AppDomain.CurrentDomain.ShadowCopyFiles)
		    {
                assemblies.AddRange(GetAssembliesRecursive(AppDomain.CurrentDomain.DynamicDirectory)); 

                return assemblies;
		    }

		    foreach (
		        var assembly in
		        GetAssemblies(AppDomain.CurrentDomain.BaseDirectory)
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
		            GetAssemblies(privateBinPath)
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
						_log.Error(string.Format("'{0}'.", exception.AllMessages()));
					}
				}
				else
				{
					_log.Error(string.Format("{0}: '{1}'.", ex.GetType(), ex.AllMessages()));
				}

				return new List<Type>();
			}

			return types;
		}
	}
}