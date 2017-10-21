using System;
using System.Configuration;

namespace Shuttle.Core.Infrastructure
{
    public class ComponentRegistryComponentElement : ConfigurationElement
    {
        [ConfigurationProperty("dependencyType", IsRequired = true)]
        public string DependencyType => (string) this["dependencyType"];

        [ConfigurationProperty("implementationType", IsRequired = false, DefaultValue = "")]
        public string ImplementationType => (string) this["implementationType"];

        [ConfigurationProperty("lifestyle", IsRequired = false, DefaultValue = "Singleton")]
        public Lifestyle Lifestyle
        {
            get
            {
                Lifestyle result;

                return Enum.TryParse(this["lifestyle"].ToString(), true, out result) ? result : Lifestyle.Singleton;
            }
        }
    }
}