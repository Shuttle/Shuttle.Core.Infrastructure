using System;

namespace Shuttle.Core.Infrastructure
{
    public class NullDependencyException: Exception
    {
        public NullDependencyException(string message) : base(message)
        {
        }
    }
}
