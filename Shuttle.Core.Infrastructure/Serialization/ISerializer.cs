using System;
using System.IO;

namespace Shuttle.Core.Infrastructure.Serialization
{
	public interface ISerializer
	{
		Stream Serialize(object instance);
		object Deserialize(Type type, Stream stream);
	}
}