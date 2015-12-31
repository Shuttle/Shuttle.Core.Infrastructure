using System;
using System.IO;
using System.Runtime.InteropServices;
using NUnit.Framework;

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
			var complex = new ComplexSerializerType();
			var serializer = new DefaultSerializer();

			serializer.AddSerializerType(typeof(ComplexSerializerType), typeof(v1.SomeSerializerType));
			serializer.AddSerializerType(typeof(ComplexSerializerType), typeof(v1.AnotherSerializerType));
			serializer.AddSerializerType(typeof(ComplexSerializerType), typeof(v2.SomeSerializerType));
			serializer.AddSerializerType(typeof(ComplexSerializerType), typeof(v2.AnotherSerializerType));

			var stream = serializer.Serialize(complex);
			var xml = new StreamReader(stream).ReadToEnd();

			Assert.IsTrue(xml.Contains(complex.Id.ToString()));

			stream.Position = 0;

			Assert.AreEqual(complex.Id, ((ComplexSerializerType) serializer.Deserialize(typeof (ComplexSerializerType), stream)).Id);

			Console.WriteLine(xml);

			var some1 = new v1.SomeSerializerType();
			var some2 = new v2.SomeSerializerType();

			Assert.AreEqual(some1.Id, ((v1.SomeSerializerType)serializer.Deserialize(typeof(v1.SomeSerializerType), serializer.Serialize(some1))).Id);
			Assert.AreEqual(some2.Id, ((v2.SomeSerializerType)serializer.Deserialize(typeof(v2.SomeSerializerType), serializer.Serialize(some2))).Id);
		}
	}
}