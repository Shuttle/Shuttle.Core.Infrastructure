using System;

namespace Shuttle.Core.Infrastructure
{
	public class PipelineEventEventArgs : EventArgs
	{
		public PipelineEventEventArgs(IPipelineEvent pipelineEvent)
		{
			PipelineEvent = pipelineEvent;
		}

		public IPipelineEvent PipelineEvent { get; private set; }
	}
}