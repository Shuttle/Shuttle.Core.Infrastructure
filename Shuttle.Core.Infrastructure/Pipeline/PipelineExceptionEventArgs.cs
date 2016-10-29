using System;

namespace Shuttle.Core.Infrastructure
{
	public class PipelineExceptionEventArgs : EventArgs
	{
		public IPipeline Pipeline { get; private set; }

		public PipelineExceptionEventArgs(IPipeline pipeline)
		{
			Pipeline = pipeline;
		}
	}
}