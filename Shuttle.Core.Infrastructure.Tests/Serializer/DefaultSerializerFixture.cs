using System;
using System.IO;
using NUnit.Framework;
using Shuttle.Core.Infrastructure.Serialization;

namespace Shuttle.Core.Infrastructure.Tests
{
	public class DefaultSerializerFixture
	{
		[Test]
		public void Should_be_able_to_serialize_and_deserialize_a_simple_type()
		{
			var original = new SimpleSerializerType();
			var serializer = new DefaultSerializer();

			var stream = serializer.Serialize(original);

			var xml = new StreamReader(stream).ReadToEnd();

			Assert.IsTrue(xml.Contains(original.Id.ToString()));

			stream.Position = 0;

			Assert.AreEqual(original.Id, ((SimpleSerializerType) serializer.Deserialize(typeof (SimpleSerializerType), stream)).Id);
		}

		[Test]
		public void Should_be_able_to_serialize_and_deserialize_a_complex_type()
		{
			var original = new ComplexSerializerType();
			var serializer = new DefaultSerializer();

			serializer.AddSerializerType(typeof(ComplexSerializerType));
			serializer.AddSerializerType(typeof(v1.SomeSerializerType));
			serializer.AddSerializerType(typeof(v1.AnotherSerializerType));
			serializer.AddSerializerType(typeof(v2.SomeSerializerType));
			serializer.AddSerializerType(typeof(v2.AnotherSerializerType));

			var stream = serializer.Serialize(original);

			var xml = new StreamReader(stream).ReadToEnd();

			Assert.IsTrue(xml.Contains(original.Id.ToString()));

			stream.Position = 0;

			Assert.AreEqual(original.Id, ((ComplexSerializerType) serializer.Deserialize(typeof (ComplexSerializerType), stream)).Id);

			Console.WriteLine(xml);
		}
	}
}