using System.Configuration;

namespace Shuttle.Core.Infrastructure
{
    public class ComponentResolverElement : ConfigurationElement
    {
        [ConfigurationProperty("dependencyType", IsRequired = true)]
        public string DependencyType
        {
            get { return (string) this["dependencyType"]; }
        }
    }
}