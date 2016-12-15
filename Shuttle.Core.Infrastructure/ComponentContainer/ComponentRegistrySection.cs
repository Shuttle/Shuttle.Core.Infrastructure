using System;
using System.Configuration;

namespace Shuttle.Core.Infrastructure
{
    public class ComponentRegistrySection : ConfigurationSection
    {
        [ConfigurationProperty("components", IsRequired = false, DefaultValue = null)]
        public ComponentRegistryCollectionElement Components
        {
            get { return (ComponentRegistryCollectionElement)this["components"]; }
        }

        public static void Register(IComponentRegistry registry)
        {
            Guard.AgainstNull(registry, "registry");

            var section = ConfigurationSectionProvider.OpenFile<ComponentRegistrySection>("shuttle", "componentRegistry");

            if (section == null)
            {
                return;
            }

            foreach (ComponentRegistryElement component in section.Components)
            {
                var serviceType = Type.GetType(component.ServiceType);
                var implementationType = string.IsNullOrEmpty(component.ImplementationType)
                    ? serviceType
                    : Type.GetType(component.ImplementationType);

                if (string.IsNullOrEmpty(component.Name))
                {
                    registry.Register(serviceType, implementationType, component.Lifestyle);
                }
                else
                {
                    registry.Register(component.Name, serviceType, implementationType, component.Lifestyle);
                }
            }
        }
    }
}