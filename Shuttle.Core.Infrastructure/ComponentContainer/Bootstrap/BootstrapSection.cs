using System.Configuration;

namespace Shuttle.Core.Infrastructure
{
    public class BootstrapSection : ConfigurationSection
    {
        [ConfigurationProperty("scan", IsRequired = false, DefaultValue = BootstrapScan.Shuttle)]
        public BootstrapScan Scan => (BootstrapScan)this["scan"];

        [ConfigurationProperty("assemblies", IsRequired = false, DefaultValue = null)]
        public BootstrapAssemblyCollectionElement Assemblies => (BootstrapAssemblyCollectionElement)this["assemblies"];

        public static IBootstrapConfiguration Configuration()
        {
            var result = new BootstrapConfiguration();
            var section = ConfigurationSectionProvider.Open<BootstrapSection>("shuttle", "bootstrap");

            if (section == null)
            {
                return result;
            }

            var reflectionService = new ReflectionService();

            result.Scan = section.Scan;

            foreach (BootstrapAssemblyElement assemblyElement in section.Assemblies)
            {
                var assembly = reflectionService.FindAssemblyNamed(assemblyElement.Name);

                if (assembly == null)
                {
                    throw new ConfigurationErrorsException(string.Format(InfrastructureResources.AssemblyNameNotFound, assemblyElement.Name));
                }

                result.AddAssembly(assembly);
            }

            return result;
        }
    }
}