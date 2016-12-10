using System;

namespace Shuttle.Core.Infrastructure
{
    public interface IComponentResolver
    {
        object Resolve(Type serviceType);
    }
}