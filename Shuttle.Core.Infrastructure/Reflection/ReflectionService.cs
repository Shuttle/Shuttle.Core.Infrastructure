using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Shuttle.Core.Infrastructure
{
    public class ReflectionService : IReflectionService
    {
	    private readonly IEnumerable<string> _ignoreAssemblies;
	    private readonly ILog _log;

	    public static IEnumerable<string> DefaultIgnoredAssemblies()
	    {
		    return new List<string>
		    {
			    "System",
			    "System.Configuration",
			    "System.Core",
			    "System.Runtime.Serialization",
			    "System.Transactions",
			    "System.Xml"
		    };
	    }

	    public ReflectionService()
			: this(DefaultIgnoredAssemblies())
	    {
	    }

	    public ReflectionService(IEnumerable<string> ignoreAssemblies)
	    {
		    _ignoreAssemblies = ignoreAssemblies ?? new List<string>();
		    _log = Log.For(this);
	    }

	    public IEnumerable<Type> GetTypes<T>()
        {
            return GetTypes(typeof (T));
        }

        public IEnumerable<Type> GetTypes(Type type)
        {
            var result = new List<Type>();

			AppDomain.CurrentDomain.GetAssemblies().ToList().ForEach(assembly => result.AddRange(GetTypes(type, assembly)));

            return result;
        }

        public IEnumerable<Type> GetTypes<T>(Assembly assembly)
        {
            return GetTypes(typeof (T), assembly);
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
    }
}