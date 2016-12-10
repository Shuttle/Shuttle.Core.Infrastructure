using System;

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
        IComponentRegistry Register(Type serviceType, object instance);
    }
}