using System;

namespace Shuttle.Core.Infrastructure
{
	public class DefaultPipelineFactory : IPipelineFactory
	{
		private readonly ReusableObjectPool<object> _pool;

        public DefaultPipelineFactory()
		{
			_pool = new ReusableObjectPool<object>();
		}

        public void OnPipelineCreated(object sender, PipelineEventArgs args)
        {
            PipelineCreated.Invoke(sender, args);
        }

        public void OnPipelineObtained(object sender, PipelineEventArgs args)
        {
            PipelineObtained.Invoke(sender, args);
        }

        public void OnPipelineReleased(object sender, PipelineEventArgs args)
        {
            PipelineReleased.Invoke(sender, args);
        }

	    public event PipelineCreatedDelegate PipelineCreated = delegate { };
	    public event PipelineObtainedDelegate PipelineObtained = delegate { };
	    public event PipelineReleaseDelegate PipelineReleased = delegate { };

	    public TPipeline GetPipeline<TPipeline>() where TPipeline : IPipeline
		{
			var pipeline = (TPipeline)_pool.Get(typeof (TPipeline));

		    if (pipeline == null)
		    {
                var type = typeof(TPipeline);

                type.AssertDefaultConstructor(string.Format(InfrastructureResources.DefaultConstructorRequired, "Pipeline", type.FullName));

                pipeline = (TPipeline)Activator.CreateInstance(type);

                OnPipelineCreated(this, new PipelineEventArgs(pipeline));
            }
		    else
		    {
                OnPipelineObtained(this, new PipelineEventArgs(pipeline));
            }

            return pipeline;
		}

		public void ReleasePipeline(IPipeline pipeline)
		{
			Guard.AgainstNull(pipeline, "pipeline");

			_pool.Release(pipeline);

            OnPipelineReleased(this, new PipelineEventArgs(pipeline));
        }
	}
}