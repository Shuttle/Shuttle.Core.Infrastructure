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
                var dependencyType = Type.GetType(component.DependencyType);
                var implementationType = string.IsNullOrEmpty(component.ImplementationType)
                    ? dependencyType
                    : Type.GetType(component.ImplementationType);

                    registry.Register(dependencyType, implementationType, component.Lifestyle);
            }

            foreach (ComponentRegistryCollectionElement collection in section.Collections)
            {
                var dependencyType = Type.GetType(collection.DependencyType);
                var implementationTypes = new List<Type>();

                foreach (ComponentRegistryCollectionImplementationTypeElement element in collection)
                {
                    implementationTypes.Add(Type.GetType(element.ImplementationType));
                }

                if (implementationTypes.Count > 0)
                {
                    registry.RegisterCollection(dependencyType, implementationTypes, collection.Lifestyle);
                }
            }
        }
    }
}