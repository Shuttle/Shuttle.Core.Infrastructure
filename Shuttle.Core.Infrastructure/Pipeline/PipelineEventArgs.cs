using System;

namespace Shuttle.Core.Infrastructure
{
	public class PipelineEventArgs : EventArgs
	{
		public IPipeline Pipeline { get; private set; }

		public PipelineEventArgs(IPipeline pipeline)
		{
			Pipeline = pipeline;
		}
	}
}