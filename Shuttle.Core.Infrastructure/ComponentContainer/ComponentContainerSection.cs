using System.Configuration;

namespace Shuttle.Core.Infrastructure
{
    public class ComponentContainerSection : ConfigurationSection
    {
        [ConfigurationProperty("components", IsRequired = false, DefaultValue = null)]
        public ComponentsElement Components
        {
            get { return (ComponentsElement)this["components"]; }
        }

    }
}