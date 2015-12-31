using System;

namespace Shuttle.Core.Infrastructure.Serialization
{
	public interface ISerializerTypes
	{
		void AddSerializerType(Type type);
	}
}