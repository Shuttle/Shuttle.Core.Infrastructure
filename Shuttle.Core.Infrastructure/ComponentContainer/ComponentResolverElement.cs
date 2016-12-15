using System.Configuration;

namespace Shuttle.Core.Infrastructure
{
    public class ComponentResolverElement : ConfigurationElement
    {
        [ConfigurationProperty("serviceType", IsRequired = true)]
        public string ServiceType
        {
            get { return (string) this["serviceType"]; }
        }
    }
}