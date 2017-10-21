using System.Collections.Generic;
using System.Reflection;

namespace Shuttle.Core.Infrastructure
{
    public interface IBootstrapConfiguration
    {
        BootstrapScan Scan { get; }
        IEnumerable<Assembly> Assemblies { get; }
    }
}