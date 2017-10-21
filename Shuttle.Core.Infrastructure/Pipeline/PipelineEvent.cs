namespace Shuttle.Core.Infrastructure
{
    public abstract class PipelineEvent : IPipelineEvent
    {
        public IPipeline Pipeline { get; private set; }

        public string Name => GetType().FullName;

        public IPipelineEvent Reset(IPipeline pipeline)
        {
            Guard.AgainstNull(pipeline, "pipeline");

            Pipeline = pipeline;

            return this;
        }
    }
}