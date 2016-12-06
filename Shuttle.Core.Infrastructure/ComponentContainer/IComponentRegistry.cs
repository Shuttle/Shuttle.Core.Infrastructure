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
        IComponentResolver Register(Type serviceType, Type implementationType, Lifestyle lifestyle);
        IComponentResolver Register(Type serviceType, object instance);
        bool IsRegistered(Type serviceType);
    }
}