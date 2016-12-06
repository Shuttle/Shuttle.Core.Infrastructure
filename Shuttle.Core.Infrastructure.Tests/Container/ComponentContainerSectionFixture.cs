using System;
using NUnit.Framework;

namespace Shuttle.Core.Infrastructure.Tests
{
	[TestFixture]
	public class ComponentContainerSectionFixture
	{
		[Test]
		[TestCase("./Container/files/ComponentContainer.config")]
		[TestCase("./Container/files/ComponentContainer-Grouped.config")]
		public void Should_be_able_to_load_the_configuration(string file)
		{
			var section = ConfigurationSectionProvider.OpenFile<ComponentRegistrySection>("shuttle", "componentRegistry", file);

			Assert.IsNotNull(section);
			Assert.IsNotNull(section.Components);
			Assert.AreEqual(2, section.Components.Count);

			foreach (ComponentElement componentElement in section.Components)
			{
				Console.WriteLine(componentElement.ServiceType);
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