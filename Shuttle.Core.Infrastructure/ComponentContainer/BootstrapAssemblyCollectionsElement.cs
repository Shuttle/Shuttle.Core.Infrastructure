using System;
using System.Configuration;

namespace Shuttle.Core.Infrastructure
{
    [ConfigurationCollection(typeof(BootstrapAssemblyElement), AddItemName = "collection",
        CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class BootstrapAssemblyCollectionsElement : ConfigurationElementCollection
	{
		protected override ConfigurationElement CreateNewElement()
		{
			return new BootstrapAssemblyElement();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return Guid.NewGuid();
		}
	}
}