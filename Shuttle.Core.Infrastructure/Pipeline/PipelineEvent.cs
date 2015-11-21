namespace Shuttle.Core.Infrastructure
{
    public abstract class PipelineEvent
    {
        public Pipeline Pipeline { get; private set; }

        public string Name
        {
            get { return GetType().FullName; }
        }

        internal PipelineEvent Reset(Pipeline pipeline)
        {
            Guard.AgainstNull(pipeline, "pipeline");

            Pipeline = pipeline;

            return this;
        }
    }
}