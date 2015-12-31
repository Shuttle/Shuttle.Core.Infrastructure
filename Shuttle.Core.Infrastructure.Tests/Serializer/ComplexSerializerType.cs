using System;

namespace Shuttle.Core.Infrastructure.Tests
{
	public class ComplexSerializerType
	{
		public ComplexSerializerType()
		{
			Id = Guid.NewGuid();
			SomeSerializerType1 = new v1.SomeSerializerType();
			AnotherSerializerType1 = new v1.AnotherSerializerType();
			SomeSerializerType2 = new v2.SomeSerializerType();
			AnotherSerializerType2 = new v2.AnotherSerializerType();
		}

		public Guid Id { get; set; }
		public v1.SomeSerializerType SomeSerializerType1 { get; set; }
		public v1.AnotherSerializerType AnotherSerializerType1 { get; set; }
		public v2.SomeSerializerType SomeSerializerType2 { get; set; }
		public v2.AnotherSerializerType AnotherSerializerType2 { get; set; }
	}
}