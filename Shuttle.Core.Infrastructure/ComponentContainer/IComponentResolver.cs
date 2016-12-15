using System;
using System.Collections;

namespace Shuttle.Core.Infrastructure
{
    public interface IComponentResolver
    {
        object Resolve(Type serviceType);
        object Resolve(string name, Type serviceType);
        IEnumerable ResolveAll(Type serviceType);
    }
}