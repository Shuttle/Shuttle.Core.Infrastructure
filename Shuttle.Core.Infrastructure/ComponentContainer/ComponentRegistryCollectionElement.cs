using System;
using System.Configuration;

namespace Shuttle.Core.Infrastructure
{
	public class ComponentRegistryCollectionElement : ConfigurationElementCollection
	{
		protected override ConfigurationElement CreateNewElement()
		{
			return new ComponentRegistryElement();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return Guid.NewGuid();
		}
	}
}