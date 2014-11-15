using System;

namespace Shuttle.Core.Infrastructure
{
    public class InvariantException : Exception
    {
        public InvariantException(string message) : base(message)
        {
        }
    }
}
