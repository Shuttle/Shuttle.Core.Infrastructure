using System;
using System.Configuration;

namespace Shuttle.Core.Infrastructure
{
	public class ComponentsElement : ConfigurationElementCollection
	{
		protected override ConfigurationElement CreateNewElement()
		{
			return new ComponentElement();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return Guid.NewGuid();
		}
	}
}