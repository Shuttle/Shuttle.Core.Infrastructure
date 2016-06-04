namespace Shuttle.Core.Infrastructure
{
    public interface IObserver
    {
    }

    public interface IPipelineObserver<in TPipelineEvent> : IObserver where TPipelineEvent : IPipelineEvent
    {
        void Execute(TPipelineEvent pipelineEvent);
    }
}