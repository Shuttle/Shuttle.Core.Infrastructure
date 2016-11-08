using System.Configuration;

namespace Shuttle.Core.Infrastructure
{
	public class ComponentElement : ConfigurationElement
	{
		[ConfigurationProperty("type", IsRequired = true)]
		public string Type
		{
			get { return (string) this["type"]; }
		}
	}
}