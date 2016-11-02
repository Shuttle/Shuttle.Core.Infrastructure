using System;

namespace Shuttle.Core.Infrastructure
{
    public class DuplicateTypeRegistrationException : Exception
    {
        public DuplicateTypeRegistrationException(string message) : base(message)
        {
        }
    }
}