using System;
using System.Collections.Generic;

namespace Shuttle.Core.Infrastructure
{
    public enum Lifestyle
    {
        Singleton = 0,
        Transient = 1
    }

    public interface IComponentRegistry
    {
	    bool IsRegistered(Type type);
        IComponentRegistry Register(Type dependencyType, Type implementationType, Lifestyle lifestyle);
        IComponentRegistry RegisterCollection(Type dependencyType, IEnumerable<Type> implementationTypes, Lifestyle lifestyle);
        IComponentRegistry Register(Type dependencyType, object instance);
    }
}