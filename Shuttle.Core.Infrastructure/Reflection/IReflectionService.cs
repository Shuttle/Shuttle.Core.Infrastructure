using System;
using System.Collections.Generic;
using System.Reflection;

namespace Shuttle.Core.Infrastructure
{
    public interface IReflectionService
    {
        string AssemblyPath(Assembly assembly);
        Assembly GetAssembly(string assemblyPath);
		IEnumerable<Assembly> GetAssemblies(string folder);
		IEnumerable<Assembly> GetAssemblies();
		IEnumerable<Type> GetTypes<T>();
		IEnumerable<Type> GetTypes(Type type);
		IEnumerable<Type> GetTypes(Assembly assembly);
		IEnumerable<Type> GetTypes<T>(Assembly assembly);
		IEnumerable<Type> GetTypes(Type type, Assembly assembly);
    }
}