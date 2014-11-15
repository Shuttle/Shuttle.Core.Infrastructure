using System;

namespace Shuttle.Core.Infrastructure
{
	public class NullValueException : Exception
	{
		public NullValueException(string message) : base(message)
		{
		}
	}
}