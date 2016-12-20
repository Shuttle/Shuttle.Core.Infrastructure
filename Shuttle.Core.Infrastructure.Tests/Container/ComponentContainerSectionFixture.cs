using System;
using NUnit.Framework;

namespace Shuttle.Core.Infrastructure.Tests
{
	[TestFixture]
	public class ComponentContainerSectionFixture
	{
		[Test]
		[TestCase("./Container/files/ComponentRegistry.config")]
		[TestCase("./Container/files/ComponentRegistry-Grouped.config")]
		public void Should_be_able_to_load_the_component_registry_section(string file)
		{
			var section = ConfigurationSectionProvider.OpenFile<ComponentRegistrySection>("shuttle", "componentRegistry", file);

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
		[TestCase("./Container/files/ComponentResolver.config")]
		[TestCase("./Container/files/ComponentResolver-Grouped.config")]
		public void Should_be_able_to_load_the_configuration(string file)
		{
			var section = ConfigurationSectionProvider.OpenFile<ComponentResolverSection>("shuttle", "componentResolver", file);

			Assert.IsNotNull(section);
			Assert.IsNotNull(section.Components);
			Assert.AreEqual(2, section.Components.Count);

			foreach (ComponentResolverElement element in section.Components)
			{
				Console.WriteLine(element.DependencyType);
			}
		}

		[Test]
		[TestCase("./Container/files/Empty.config")]
		public void Should_be_able_to_handle_missing_element(string file)
		{
            var section = ConfigurationSectionProvider.OpenFile<ComponentRegistrySection>("componentRegistry", file);

            Assert.IsNotNull(section);
			Assert.IsEmpty(section.Components);
		}
	}
}