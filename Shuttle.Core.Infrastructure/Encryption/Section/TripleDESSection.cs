using System.Configuration;

namespace Shuttle.Core.Infrastructure
{
    public class TripleDESSection : ConfigurationSection
    {
        [ConfigurationProperty("key", IsRequired = false, DefaultValue = null)]
        public string Key => (string) this["key"];
    }
}