using System;
using System.Security.Cryptography.X509Certificates;

namespace Shuttle.Core.Infrastructure
{
    public delegate void PipelineCreatedDelegate(object sender, PipelineEventArgs e);

    public delegate void PipelineObtainedDelegate(object sender, PipelineEventArgs e);

    public delegate void PipelineReleaseDelegate(object sender, PipelineEventArgs e);

    public interface IPipelineFactory
	{
        void OnPipelineCreated(object sender, PipelineEventArgs args);
        void OnPipelineObtained(object sender, PipelineEventArgs args);
        void OnPipelineReleased(object sender, PipelineEventArgs args);

        event PipelineCreatedDelegate PipelineCreated;
        event PipelineObtainedDelegate PipelineObtained;
        event PipelineReleaseDelegate PipelineReleased;

        TPipeline GetPipeline<TPipeline>() where TPipeline : IPipeline;
		void ReleasePipeline(IPipeline pipeline);
	}
}