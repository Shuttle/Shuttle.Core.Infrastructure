using System;

namespace Shuttle.Core.Infrastructure
{
    public class TypeResolutionException : Exception
    {
        public TypeResolutionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public TypeResolutionException(string message) : base(message)
        {
        }
    }
}