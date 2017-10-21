using System;
using System.Configuration;

namespace Shuttle.Core.Infrastructure
{
    public class ComponentResolverCollectionElement : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ComponentResolverElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return Guid.NewGuid();
        }
    }
}