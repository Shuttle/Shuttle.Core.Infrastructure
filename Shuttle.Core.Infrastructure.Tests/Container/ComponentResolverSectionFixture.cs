using System;
using System.IO;
using NUnit.Framework;

namespace Shuttle.Core.Infrastructure.Tests
{
	[TestFixture]
	public class ComponentResolverSectionFixture
	{
	    private ComponentResolverSection GetSection(string file)
	    {
	        return ConfigurationSectionProvider.OpenFile<ComponentResolverSection>("shuttle", "componentResolver",
	            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $@".\Container\files\{file}"));
	    }

		[Test]
		[TestCase("ComponentResolver.config")]
		[TestCase("ComponentResolver-Grouped.config")]
		public void Should_be_able_to_load_the_configuration(string file)
		{
            var section = GetSection(file);

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
            var section = GetSection(file);

            Assert.IsNotNull(section);
            Assert.IsEmpty(section.Components);
		}
	}
}