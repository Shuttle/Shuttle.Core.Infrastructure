using System;
using System.Collections.Generic;
using System.Configuration;

namespace Shuttle.Core.Infrastructure
{
    public class ComponentRegistrySection : ConfigurationSection
    {
        [ConfigurationProperty("collections", IsRequired = false, DefaultValue = null)]
        public ComponentRegistryCollectionsElement Collections
        {
            get { return (ComponentRegistryCollectionsElement)this["collections"]; }
        }

        [ConfigurationProperty("components", IsRequired = false, DefaultValue = null)]
        public ComponentRegistryComponentsElement Components
        {
            get { return (ComponentRegistryComponentsElement)this["components"]; }
        }

        public static void Register(IComponentRegistry registry)
        {
            Guard.AgainstNull(registry, "registry");

            var section = ConfigurationSectionProvider.OpenFile<ComponentRegistrySection>("shuttle", "componentRegistry");

            if (section == null)
            {
                return;
            }

            foreach (ComponentRegistryComponentElement component in section.Components)
            {
                var serviceType = Type.GetType(component.ServiceType);
                var implementationType = string.IsNullOrEmpty(component.ImplementationType)
                    ? serviceType
                    : Type.GetType(component.ImplementationType);

                    registry.Register(serviceType, implementationType, component.Lifestyle);
            }

            foreach (ComponentRegistryCollectionElement collection in section.Collections)
            {
                var serviceType = Type.GetType(collection.ServiceType);
                var implementationTypes = new List<Type>();

                foreach (ComponentRegistryCollectionImplementationTypeElement element in collection)
                {
                    implementationTypes.Add(Type.GetType(element.ImplementationType));
                }

                if (implementationTypes.Count > 0)
                {
                    registry.RegisterCollection(serviceType, implementationTypes, collection.Lifestyle);
                }
            }
        }
    }
}