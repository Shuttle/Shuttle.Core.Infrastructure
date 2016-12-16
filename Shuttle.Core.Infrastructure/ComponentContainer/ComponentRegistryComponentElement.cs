using System;
using System.Configuration;

namespace Shuttle.Core.Infrastructure
{
	public class ComponentRegistryComponentElement : ConfigurationElement
	{
		[ConfigurationProperty("serviceType", IsRequired = true)]
		public string ServiceType
		{
			get { return (string) this["serviceType"]; }
		}

        [ConfigurationProperty("implementationType", IsRequired = false, DefaultValue = "")]
		public string ImplementationType
        {
			get { return (string) this["implementationType"]; }
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
	}
}