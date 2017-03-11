using System;
using System.IO;
using NUnit.Framework;

namespace Shuttle.Core.Infrastructure.Tests
{
	[TestFixture]
	public class ReflectionServiceFixture
	{
		[Test]
		public void Should_be_able_to_get_assembly_path()
		{
			var service = new ReflectionService();

			Assert.AreEqual(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Shuttle.Core.Infrastructure.Tests.dll").ToLower(), service.AssemblyPath(GetType().Assembly).ToLower());
		}
	}
}