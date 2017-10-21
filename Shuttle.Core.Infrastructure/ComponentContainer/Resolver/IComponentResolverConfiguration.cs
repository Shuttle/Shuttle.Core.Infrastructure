using System.Collections.Generic;

namespace Shuttle.Core.Infrastructure
{
    public interface IComponentResolverConfiguration
    {
        IEnumerable<ComponentResolverConfiguration.Component> Components { get; }
    }
}