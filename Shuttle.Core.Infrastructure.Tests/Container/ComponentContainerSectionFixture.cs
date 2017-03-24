using System;
using System.IO;
using NUnit.Framework;

namespace Shuttle.Core.Infrastructure.Tests
{
	[TestFixture]
	public class ComponentContainerSectionFixture
	{
	    private ComponentRegistrySection GetRegistrySection(string file)
	    {
	        return ConfigurationSectionProvider.OpenFile<ComponentRegistrySection>("shuttle", "componentRegistry",
	            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@".\Container\files\{0}", file)));
	    }

	    private ComponentResolverSection GetResolverSection(string file)
	    {
	        return ConfigurationSectionProvider.OpenFile<ComponentResolverSection>("shuttle", "componentResolver",
	            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@".\Container\files\{0}", file)));
	    }

	    [Test]
		[TestCase("ComponentRegistry.config")]
		[TestCase("ComponentRegistry-Grouped.config")]
		public void Should_be_able_to_load_the_component_registry_section(string file)
		{
			var section = GetRegistrySection(file);

			Assert.IsNotNull(section);
			Assert.IsNotNull(section.Components);
			Assert.AreEqual(2, section.Components.Count);

			foreach (ComponentRegistryComponentElement element in section.Components)
			{
				Console.WriteLine("[component] : {0}", element.DependencyType);
			}

			foreach (ComponentRegistryCollectionElement element in section.Collections)
			{
				Console.WriteLine("[collection] : {0}", element.DependencyType);

			    foreach (ComponentRegistryCollectionImplementationTypeElement typeElement in element)
			    {
                    Console.WriteLine("--- {0}", typeElement.ImplementationType);
                }
			}
		}

		[Test]
		[TestCase("ComponentResolver.config")]
		[TestCase("ComponentResolver-Grouped.config")]
		public void Should_be_able_to_load_the_configuration(string file)
		{
            var section = GetResolverSection(file);

            Assert.IsNotNull(section);
			Assert.IsNotNull(section.Components);
			Assert.AreEqual(2, section.Components.Count);

			foreach (ComponentResolverElement element in section.Components)
			{
				Console.WriteLine(element.DependencyType);
			}
		}

		[Test]
		[TestCase("Empty.config")]
		public void Should_be_able_to_handle_missing_element(string file)
		{
            var section = GetRegistrySection(file);

            Assert.IsNotNull(section);
			Assert.IsEmpty(section.Components);
		}
	}
}