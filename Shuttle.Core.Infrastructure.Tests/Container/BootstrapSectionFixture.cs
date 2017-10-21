using System;
using System.IO;
using NUnit.Framework;

namespace Shuttle.Core.Infrastructure.Tests
{
    [TestFixture]
    public class BootstrapSectionFixture
    {
        private BootstrapSection GetSection(string file)
        {
            return ConfigurationSectionProvider.OpenFile<BootstrapSection>("shuttle", "bootstrap",
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $@".\Container\files\{file}"));
        }

        [Test]
        [TestCase("Empty.config")]
        public void Should_be_able_to_handle_missing_element(string file)
        {
            var section = GetSection(file);

            Assert.IsNotNull(section);
            Assert.AreEqual(BootstrapScan.Shuttle, section.Scan);
            Assert.IsEmpty(section.Assemblies);
        }

        [Test]
        [TestCase("Bootstrap.config")]
        [TestCase("Bootstrap-Grouped.config")]
        public void Should_be_able_to_load_the_bootstrap_section(string file)
        {
            var section = GetSection(file);

            Assert.IsNotNull(section);
            Assert.IsNotNull(section.Assemblies);
            Assert.AreEqual(BootstrapScan.All, section.Scan);

            foreach (BootstrapAssemblyElement element in section.Assemblies)
            {
                Console.WriteLine(@"[assembly] : {0}", element.Name);
            }
        }
    }
}