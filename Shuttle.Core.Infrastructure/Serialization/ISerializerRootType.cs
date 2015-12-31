using System;

namespace Shuttle.Core.Infrastructure
{
	public interface ISerializerRootType
	{
		void AddSerializerType(Type root, Type contained);
	}
}