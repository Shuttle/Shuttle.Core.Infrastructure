using System;

namespace Shuttle.Core.Infrastructure
{
    public interface IPipelineFactory
    {
        void OnPipelineCreated(object sender, PipelineEventArgs args);
        void OnPipelineObtained(object sender, PipelineEventArgs args);
        void OnPipelineReleased(object sender, PipelineEventArgs args);

        event EventHandler<PipelineEventArgs> PipelineCreated;
        event EventHandler<PipelineEventArgs> PipelineObtained;
        event EventHandler<PipelineEventArgs> PipelineReleased;

        TPipeline GetPipeline<TPipeline>() where TPipeline : IPipeline;
        void ReleasePipeline(IPipeline pipeline);
    }
}