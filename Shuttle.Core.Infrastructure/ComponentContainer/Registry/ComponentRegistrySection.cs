using System;
using System.Collections.Generic;
using System.Configuration;

namespace Shuttle.Core.Infrastructure
{
    public class ComponentRegistrySection : ConfigurationSection
    {
        [ConfigurationProperty("collections", IsRequired = false, DefaultValue = null)]
        public ComponentRegistryCollectionsElement Collections => (ComponentRegistryCollectionsElement) this[
            "collections"];

        [ConfigurationProperty("components", IsRequired = false, DefaultValue = null)]
        public ComponentRegistryComponentsElement Components => (ComponentRegistryComponentsElement) this["components"];

        public static IComponentRegistryConfiguration Configuration()
        {
            var result = new ComponentRegistryConfiguration();

            var section = ConfigurationSectionProvider.Open<ComponentRegistrySection>("shuttle", "componentRegistry");

            if (section == null)
            {
                return result;
            }

            foreach (ComponentRegistryComponentElement component in section.Components)
            {
                var dependencyType = Type.GetType(component.DependencyType);

                if (dependencyType == null)
                {
                    throw new ConfigurationErrorsException(string.Format(InfrastructureResources.MissingTypeException, component.DependencyType));
                }

                var implementationType = string.IsNullOrEmpty(component.ImplementationType)
                    ? dependencyType
                    : Type.GetType(component.ImplementationType);

                if (implementationType == null)
                {
                    throw new ConfigurationErrorsException(string.Format(InfrastructureResources.MissingTypeException, component.ImplementationType));
                }

                result.AddComponent(
                    new ComponentRegistryConfiguration.Component(dependencyType, implementationType, component.Lifestyle));
            }

            foreach (ComponentRegistryCollectionElement collection in section.Collections)
            {
                var dependencyType = Type.GetType(collection.DependencyType);
                var implementationTypes = new List<Type>();

                if (dependencyType == null)
                {
                    throw new ConfigurationErrorsException(string.Format(InfrastructureResources.MissingTypeException,
                        collection.DependencyType));
                }

                foreach (ComponentRegistryCollectionImplementationTypeElement element in collection)
                {
                    var implementationType = Type.GetType(element.ImplementationType);

                    if (implementationType == null)
                    {
                        throw new ConfigurationErrorsException(string.Format(
                            InfrastructureResources.MissingTypeException, element.ImplementationType));
                    }

                    implementationTypes.Add(implementationType);
                }

                result.AddCollection(
                    new ComponentRegistryConfiguration.Collection(dependencyType, implementationTypes, collection.Lifestyle));
            }

            return result;
        }
    }
}