using System;

namespace Shuttle.Core.Infrastructure
{
    public class DuplicateEntityException : Exception
    {
        public DuplicateEntityException(string message) : base(message)
        {
        }
    }
}
