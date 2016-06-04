using System;

namespace Shuttle.Core.Infrastructure
{
    public class PipelineException : Exception
    {
        public PipelineException(string message) : base(message)
        {
        }

        public PipelineException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}