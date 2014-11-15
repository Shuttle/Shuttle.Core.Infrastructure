using System;

namespace Shuttle.Core.Infrastructure
{
    public class ConventionException : Exception
    {
        public ConventionException(string message) : base(message)
        {
        }
    }
}
