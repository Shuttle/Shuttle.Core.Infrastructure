using System;

namespace Shuttle.Core.Infrastructure
{
    public class TypeRegistrationException : Exception
    {
        public TypeRegistrationException(string message) : base(message)
        {
        }

        public TypeRegistrationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}