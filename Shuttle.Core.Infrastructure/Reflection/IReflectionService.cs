using System;
using System.Collections.Generic;
using System.Reflection;

namespace Shuttle.Core.Infrastructure
{
    public interface IReflectionService
    {
        Assembly GetAssembly(string assembly);
        IEnumerable<Assembly> GetAssemblies(string folder);
        IEnumerable<Assembly> GetAssembliesRecursive();
        IEnumerable<Assembly> GetAssembliesRecursive(string folder);
        IEnumerable<Type> GetTypes<T>();
        IEnumerable<Type> GetTypes(Type type);
		IEnumerable<Type> GetTypes(Assembly assembly);
		IEnumerable<Type> GetTypes<T>(Assembly assembly);
        IEnumerable<Type> GetTypes(Type type, Assembly assembly);
        Type GetType(Func<Type, bool> condition);
        Type GetType(Assembly assembly, Func<Type, bool> condition);
        IEnumerable<T> CreateInstances<T>(IEnumerable<Type> types);
    }
}
