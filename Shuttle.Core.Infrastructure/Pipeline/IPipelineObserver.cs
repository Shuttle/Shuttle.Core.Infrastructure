namespace Shuttle.Core.Infrastructure
{
    public interface IObserver
    {
    }

    public interface IPipelineObserver<TPipelineEvent> : IObserver
    {
        void Execute(TPipelineEvent pipelineEvent1);
    }
}