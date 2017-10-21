using System.Collections.Generic;
using System.Reflection;

namespace Shuttle.Core.Infrastructure
{
    public static class BootstrapAssemblyElementExtensions
    {
        public static IEnumerable<Assembly> GetAssemblies(this BootstrapAssemblyCollectionElement bootstrapAssemblies,
            BootstrapScan bootstrapScan)
        {
            var assemblies = new List<Assembly>();

            var reflectionService = new ReflectionService();

            switch (bootstrapScan)
            {
                case BootstrapScan.All:
                {
                    assemblies.AddRange(reflectionService.GetAssemblies());

                    break;
                }
                case BootstrapScan.Shuttle:
                {
                    assemblies.AddRange(reflectionService.GetMatchingAssemblies("^Shuttle\\."));

                    break;
                }
            }

            foreach (BootstrapAssemblyElement bootstrapAssembly in bootstrapAssemblies)
            {
                var assembly = reflectionService.FindAssemblyNamed(bootstrapAssembly.Name);

                if (assembly != null)
                {
                    assemblies.Add(assembly);
                }
            }

            return assemblies;
        }
    }
}