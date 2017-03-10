using System;
using System.Collections.Generic;
using System.Configuration;

namespace Shuttle.Core.Infrastructure
{
    public class ComponentResolverSection : ConfigurationSection
    {
        [ConfigurationProperty("components", IsRequired = false, DefaultValue = null)]
        public ComponentResolverCollectionElement Components
        {
            get { return (ComponentResolverCollectionElement)this["components"]; }
        }

        public static IEnumerable<object> Resolve(IComponentResolver resolver)
        {
            Guard.AgainstNull(resolver, "resolver");

            var result = new List<object>();
            var section = ConfigurationSectionProvider.Open<ComponentResolverSection>("shuttle", "componentResolver");

            if (section == null)
            {
                return result;
            }

            foreach (ComponentResolverElement component in section.Components)
            {
                var type = Type.GetType(component.DependencyType);

                if (type == null)
                {
                    throw new ConfigurationErrorsException(string.Format(InfrastructureResources.MissingTypeException, component.DependencyType));
                }

                result.Add(resolver.ResolveAll(type));
            }

            return result;
        }
    }
}