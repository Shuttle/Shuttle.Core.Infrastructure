using System;
using System.IO;
using NUnit.Framework;

namespace Shuttle.Core.Infrastructure.Tests
{
	[TestFixture]
	public class ComponentRegistrySectionFixture
    {
	    private ComponentRegistrySection GetSection(string file)
	    {
	        return ConfigurationSectionProvider.OpenFile<ComponentRegistrySection>("shuttle", "componentRegistry",
	            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $@".\Container\files\{file}"));
	    }

	    [Test]
		[TestCase("ComponentRegistry.config")]
		[TestCase("ComponentRegistry-Grouped.config")]
		public void Should_be_able_to_load_the_component_registry_section(string file)
		{
			var section = GetSection(file);

			Assert.IsNotNull(section);
			Assert.IsNotNull(section.Components);
			Assert.AreEqual(2, section.Components.Count);

			foreach (ComponentRegistryComponentElement element in section.Components)
			{
				Console.WriteLine(@"[collection] : {0}", element.DependencyType);
			}

			foreach (ComponentRegistryCollectionElement element in section.Collections)
			{
				Console.WriteLine(@"[collection] : {0}", element.DependencyType);

			    foreach (ComponentRegistryCollectionImplementationTypeElement typeElement in element)
			    {
                    Console.WriteLine(@"[collection] : {0}", typeElement.ImplementationType);
                }
			}
		}

		[Test]
		[TestCase("Empty.config")]
		public void Should_be_able_to_handle_missing_element(string file)
		{
            var section = GetSection(file);

            Assert.IsNotNull(section);
            Assert.IsEmpty(section.Collections);
            Assert.IsEmpty(section.Components);
		}
	}
}