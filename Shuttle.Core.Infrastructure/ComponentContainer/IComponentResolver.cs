using System;
using System.Collections.Generic;

namespace Shuttle.Core.Infrastructure
{
    public interface IComponentResolver
    {
        object Resolve(Type serviceType);
        IEnumerable<object> ResolveAll(Type serviceType);
    }
}