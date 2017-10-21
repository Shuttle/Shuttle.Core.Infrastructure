using System.Collections.Generic;

namespace Shuttle.Core.Infrastructure
{
    public interface IComponentRegistryConfiguration
    {
        IEnumerable<ComponentRegistryConfiguration.Component> Components { get; }
        IEnumerable<ComponentRegistryConfiguration.Collection> Collections { get; }
    }
}