using System.Collections.Generic;
using System.Reflection;

namespace Shuttle.Core.Infrastructure
{
    public static class BootstrapAssemblyElementExtensions
    {
        public static IEnumerable<Assembly> GetAssemblies(this BootstrapAssemblyCollectionsElement bootstrapAssemblies, BootstrapAssemblyScan bootstrapAssemblyScan)
        {
            var assemblies = new List<Assembly>();

            var reflectionService = new ReflectionService();

            switch (bootstrapAssemblyScan)
            {
                case BootstrapAssemblyScan.All:
                    {
                        assemblies.AddRange(reflectionService.GetAssemblies());

                        break;
                    }
                case BootstrapAssemblyScan.Shuttle:
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