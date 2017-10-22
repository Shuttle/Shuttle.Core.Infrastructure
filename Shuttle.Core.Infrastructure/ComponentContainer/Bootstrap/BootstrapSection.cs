using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;

namespace Shuttle.Core.Infrastructure
{
    public class BootstrapSection : ConfigurationSection
    {
        [ConfigurationProperty("scan", IsRequired = false, DefaultValue = BootstrapScan.Shuttle)]
        public BootstrapScan Scan => (BootstrapScan)this["scan"];

        [ConfigurationProperty("assemblies", IsRequired = false, DefaultValue = null)]
        public BootstrapAssembliesElement Assemblies => (BootstrapAssembliesElement)this["assemblies"];

        public static IBootstrapConfiguration Configuration()
        {
            var result = new BootstrapConfiguration();
            var section = ConfigurationSectionProvider.Open<BootstrapSection>("shuttle", "bootstrap");

            var reflectionService = new ReflectionService();

            if (section != null)
            {
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
            }

            if (result.Scan != BootstrapScan.None)
            {
                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    result.AddAssembly(assembly);
                }
            }

            switch (result.Scan)
            {
                case BootstrapScan.All:
                {
                    foreach (var assembly in reflectionService.GetAssemblies())
                    {
                        result.AddAssembly(assembly);
                    }
                    break;
                }
                case BootstrapScan.Shuttle:
                {
                    foreach (var assembly in reflectionService.GetMatchingAssemblies("^Shuttle\\."))
                    {
                        result.AddAssembly(assembly);
                    }
                    break;
                }
            }

            return result;
        }
    }
}