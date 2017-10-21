using System;
using System.Configuration;

namespace Shuttle.Core.Infrastructure
{
    [ConfigurationCollection(typeof(ComponentRegistryCollectionElement), AddItemName = "collection",
        CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class ComponentRegistryCollectionsElement : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ComponentRegistryCollectionElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return Guid.NewGuid();
        }
    }
}