using System;

namespace Shuttle.Core.Infrastructure
{
    public class MissingEntityException : Exception
    {
        public MissingEntityException(string message) : base(message)
        {
        }
    }
}
