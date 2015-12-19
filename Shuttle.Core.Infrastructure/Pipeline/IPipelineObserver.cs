namespace Shuttle.Core.Infrastructure
{
    public interface IObserver
    {
    }

    public interface IPipelineObserver<in TPipelineEvent> : IObserver
    {
        void Execute(TPipelineEvent pipelineEvent1);
    }
}