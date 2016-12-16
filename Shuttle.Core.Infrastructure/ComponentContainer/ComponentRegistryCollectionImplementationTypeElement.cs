using System.Configuration;

namespace Shuttle.Core.Infrastructure
{
    public class ComponentRegistryCollectionImplementationTypeElement : ConfigurationElement
    {
        [ConfigurationProperty("implementationType", IsRequired = false, DefaultValue = "")]
        public string ImplementationType
        {
            get { return (string)this["implementationType"]; }
        }

    }
}