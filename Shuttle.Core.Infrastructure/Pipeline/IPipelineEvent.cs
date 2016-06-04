namespace Shuttle.Core.Infrastructure
{
    public interface IPipelineEvent
    {
        IPipeline Pipeline { get; }
        string Name { get; }
        IPipelineEvent Reset(IPipeline pipeline);
    }
}