using System;

namespace Shuttle.Core.Infrastructure
{
	public class EmptyStringException : Exception
	{
		public EmptyStringException(string message) : base(message)
		{
		}
	}
}