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
        IComponentRegistry Register(Type serviceType, Type implementationType, Lifestyle lifestyle);
        IComponentRegistry RegisterCollection(Type serviceType, IEnumerable<Type> implementationTypes, Lifestyle lifestyle);
        IComponentRegistry Register(Type serviceType, object instance);
    }
}