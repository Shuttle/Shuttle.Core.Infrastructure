using System;

namespace Shuttle.Core.Infrastructure
{
    public class ReassignmentException : Exception
    {
        public ReassignmentException(string message) : base(message)
        {
        }
    }
}
