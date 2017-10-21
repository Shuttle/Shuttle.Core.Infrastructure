using NUnit.Framework;

namespace Shuttle.Core.Infrastructure.Tests
{
    [TestFixture]
    public class ConfigurationSectionProviderFixture
    {
        [Test]
        public void Should_be_able_to_open_a_section()
        {
            var registry = ConfigurationSectionProvider.Open<ComponentRegistrySection>("shuttle", "componentRegistry");

            Assert.IsNotNull(registry);
        }

        [Test]
        public void Should_be_able_to_open_a_grouped_section()
        {
            var resolver = ConfigurationSectionProvider.Open<ComponentResolverSection>("shuttle", "componentResolver");

            Assert.IsNotNull(resolver);
        }
    }
}