using System;

namespace Shuttle.Core.Infrastructure
{
    public class PipelineExceptionEventArgs : EventArgs
    {
        public PipelineExceptionEventArgs(IPipeline pipeline)
        {
            Pipeline = pipeline;
        }

        public IPipeline Pipeline { get; }
    }
}