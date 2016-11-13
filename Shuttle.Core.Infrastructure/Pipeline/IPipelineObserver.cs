namespace Shuttle.Core.Infrastructure
{
    public interface IPipelineObserver
    {
    }

    public interface IPipelineObserver<in TPipelineEvent> : IPipelineObserver where TPipelineEvent : IPipelineEvent
    {
        void Execute(TPipelineEvent pipelineEvent);
    }
}