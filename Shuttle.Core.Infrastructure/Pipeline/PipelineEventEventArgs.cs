namespace Shuttle.Core.Infrastructure
{
	public class PipelineEventEventArgs
	{
		public PipelineEventEventArgs(IPipelineEvent pipelineEvent)
		{
			PipelineEvent = pipelineEvent;
		}

		public IPipelineEvent PipelineEvent { get; private set; }
	}
}