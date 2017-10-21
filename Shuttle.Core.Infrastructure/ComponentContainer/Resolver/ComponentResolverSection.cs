using System;
using System.Configuration;

namespace Shuttle.Core.Infrastructure
{
    public class ComponentResolverSection : ConfigurationSection
    {
        [ConfigurationProperty("bootstrapAssemblyScan", IsRequired = false,
            DefaultValue = BootstrapScan.Shuttle)]
        public BootstrapScan BootstrapScan => (BootstrapScan) this["bootstrapAssemblyScan"];

        [ConfigurationProperty("components", IsRequired = false, DefaultValue = null)]
        public ComponentResolverCollectionElement Components => (ComponentResolverCollectionElement) this["components"];

        [ConfigurationProperty("bootstrapAssemblies", IsRequired = false, DefaultValue = null)]
        public BootstrapAssemblyCollectionElement BootstrapAssemblies => (BootstrapAssemblyCollectionElement) this[
            "bootstrapAssemblies"];

        public static IComponentResolverConfiguration Configuration()
        {
            var result = new ComponentResolverConfiguration();
            var section = ConfigurationSectionProvider.Open<ComponentResolverSection>("shuttle", "componentResolver");

            if (section == null)
            {
                return result;
            }

            foreach (ComponentResolverElement component in section.Components)
            {
                var dependencyType = Type.GetType(component.DependencyType);

                if (dependencyType == null)
                {
                    throw new ConfigurationErrorsException(
                        string.Format(InfrastructureResources.MissingTypeException, component.DependencyType));
                }

                result.AddComponent(new ComponentResolverConfiguration.Component(dependencyType));
            }

            return result;
        }
    }
}