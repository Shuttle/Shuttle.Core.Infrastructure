using System;

namespace Shuttle.Core.Infrastructure
{
    public class PipelineEventArgs : EventArgs
    {
        public PipelineEventArgs(IPipeline pipeline)
        {
            Pipeline = pipeline;
        }

        public IPipeline Pipeline { get; }
    }
}