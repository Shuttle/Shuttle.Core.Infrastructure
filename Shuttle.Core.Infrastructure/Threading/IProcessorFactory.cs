namespace Shuttle.Core.Infrastructure
{
    public interface IProcessorFactory
    {
        IProcessor Create();
    }
}