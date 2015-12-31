using System;

namespace Shuttle.Core.Infrastructure.Tests.v1
{
	public class AnotherSerializerType
	{
		public AnotherSerializerType()
		{
			Id = Guid.NewGuid();
		}

		public Guid Id { get; set; }
	}
}