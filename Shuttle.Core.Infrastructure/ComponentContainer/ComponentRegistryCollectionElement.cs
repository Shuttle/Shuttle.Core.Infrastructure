using System;
using System.Configuration;

namespace Shuttle.Core.Infrastructure
{
    public class ComponentRegistryCollectionElement : ConfigurationElementCollection
    {
		[ConfigurationProperty("serviceType", IsRequired = true)]
		public string ServiceType
		{
			get { return (string) this["serviceType"]; }
		}

	    [ConfigurationProperty("lifestyle", IsRequired = false, DefaultValue = "Singleton")]
	    public Lifestyle Lifestyle
	    {
            get
            {
                Lifestyle result;

                return Enum.TryParse(this["lifestyle"].ToString(), true, out result) ? result : Lifestyle.Singleton;
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ComponentRegistryCollectionImplementationTypeElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return Guid.NewGuid();
        }
    }
}