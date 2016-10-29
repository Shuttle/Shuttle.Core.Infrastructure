namespace Shuttle.Core.Infrastructure
{
	public interface IPipelineModule
	{
	    void Start(IPipelineFactory pipelineFactory);
	}
}