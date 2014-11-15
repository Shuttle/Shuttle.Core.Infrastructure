using System;

namespace Shuttle.Core.Infrastructure
{
    public class DuplicateEntryException : Exception
    {
        public DuplicateEntryException(string message) : base(message)
        {
        }
    }
}
