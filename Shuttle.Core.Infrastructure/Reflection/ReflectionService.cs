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

		public Assembly GetAssembly(string assembly)
		{
			Assembly scanAssembly;

			try
			{
				switch (Path.GetExtension(assembly))
				{
					case ".dll":
					case ".exe":
						{
							scanAssembly = Path.GetDirectoryName(assembly) == AppDomain.CurrentDomain.BaseDirectory
											   ? Assembly.Load(Path.GetFileNameWithoutExtension(assembly))
											   : Assembly.LoadFile(assembly);
							break;
						}

					default:
						{
							scanAssembly = Assembly.Load(assembly);

							break;
						}
				}
			}
			catch (Exception ex)
			{
				_log.Warning(string.Format("[GetAssembly] : could not load assembly '{0}' / exception = {1}", assembly, ex.Message));

				return null;
			}

			return scanAssembly;
		}

		public IEnumerable<Assembly> GetAssemblies(string folder)
		{
			var result = new List<Assembly>();

			if (Directory.Exists(folder))
			{
				result.AddRange(Directory.GetFiles(folder, "*.exe").Select(GetAssembly).Where(assembly => assembly != null));
				result.AddRange(Directory.GetFiles(folder, "*.dll").Select(GetAssembly).Where(assembly => assembly != null));
			}

			return result;
		}

		public IEnumerable<Assembly> GetAssembliesRecursive()
		{
			var assemblies = new List<Assembly>(AppDomain.CurrentDomain.GetAssemblies());

			foreach (var assembly in GetAssembliesRecursive(AppDomain.CurrentDomain.BaseDirectory).Where(assembly => assemblies.Find(candidate => candidate.Equals(assembly)) == null))
			{
				assemblies.Add(assembly);
			}

			var privateBinPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
											  AppDomain.CurrentDomain.RelativeSearchPath ?? string.Empty);

			if (!privateBinPath.Equals(AppDomain.CurrentDomain.BaseDirectory))
			{
				foreach (var assembly in GetAssembliesRecursive(privateBinPath).Where(assembly => assemblies.Find(candidate => candidate.Equals(assembly)) == null))
				{
					assemblies.Add(assembly);
				}
			}

			return assemblies;
		}

		public IEnumerable<Assembly> GetAssembliesRecursive(string folder)
		{
			var result = new List<Assembly>(GetAssemblies(folder));

			foreach (var directory in Directory.GetDirectories(folder))
			{
				result.AddRange(GetAssembliesRecursive(directory));
			}

			return result;
		}

		public IEnumerable<Type> GetTypes<T>()
		{
			return GetTypes(typeof(T));
		}

		public IEnumerable<Type> GetTypes(Type type)
		{
			var result = new List<Type>();

			var assemblies = new List<Assembly>(AppDomain.CurrentDomain.GetAssemblies());

			foreach (var assembly in GetAssembliesRecursive(AppDomain.CurrentDomain.BaseDirectory).Where(assembly => assemblies.Find(candidate => candidate.Equals(assembly)) == null))
			{
				assemblies.Add(assembly);
			}

			var privateBinPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
											  AppDomain.CurrentDomain.RelativeSearchPath ?? string.Empty);

			if (!privateBinPath.Equals(AppDomain.CurrentDomain.BaseDirectory))
			{
				foreach (var assembly in GetAssembliesRecursive(privateBinPath).Where(assembly => assemblies.Find(candidate => candidate.Equals(assembly)) == null))
				{
					assemblies.Add(assembly);
				}
			}

			assemblies.ForEach(assembly => result.AddRange(GetTypes(type, assembly)));

			return result;
		}

		public IEnumerable<Type> GetTypes<T>(Assembly assembly)
		{
			return GetTypes(typeof(T), assembly);
		}

		public IEnumerable<Type> GetTypes(Type type, Assembly assembly)
		{
			Guard.AgainstNull(type, "type");
			Guard.AgainstNull(assembly, "assembly");

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

		public Type GetType(Func<Type, bool> condition)
		{
			Guard.AgainstNull(condition, "condition");

			return GetAssembliesRecursive().Select(assembly => GetType(assembly, condition)).FirstOrDefault(type => type != null);
		}

		public Type GetType(Assembly assembly, Func<Type, bool> condition)
		{
			Guard.AgainstNull(assembly, "assembly");
			Guard.AgainstNull(condition, "condition");

			return GetTypes(assembly).FirstOrDefault(condition.Invoke);
		}

		public IEnumerable<T> CreateInstances<T>(IEnumerable<Type> types)
		{
			var result = new List<T>();

			foreach (var type in types)
			{
				type.AssertDefaultConstructor();

				result.Add((T)Activator.CreateInstance(type));
			}

			return result;
		}
	}
}