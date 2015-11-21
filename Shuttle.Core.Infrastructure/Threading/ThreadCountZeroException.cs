using System;

namespace Shuttle.Core.Infrastructure
{
    public class ThreadCountZeroException : Exception
    {
        public ThreadCountZeroException()
            : base(InfrastructureResources.ThreadCountZeroException)
        {
        }
    }
}