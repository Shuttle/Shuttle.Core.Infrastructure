using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Shuttle.Core.Infrastructure
{
    public class BootstrapConfiguration : IBootstrapConfiguration
    {
        private readonly IList<Assembly> _assemblies = new List<Assembly>();

        public BootstrapConfiguration()
        {
            Scan = BootstrapScan.Shuttle;
        }

        public BootstrapScan Scan { get; set; }
        public IEnumerable<Assembly> Assemblies => new ReadOnlyCollection<Assembly>(_assemblies);

        public void AddAssembly(Assembly assembly)
        {
            Guard.AgainstNull(assembly, nameof(assembly));

            _assemblies.Add(assembly);
        }
    }
}