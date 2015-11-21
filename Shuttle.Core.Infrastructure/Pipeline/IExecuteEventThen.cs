namespace Shuttle.Core.Infrastructure
{
    public interface IExecuteEventThen
    {
        IExecuteEventThen ThenEvent(PipelineEvent pipelineEvent);
        IExecuteEventThen ThenEvent<TPipelineEvent>() where TPipelineEvent : PipelineEvent, new();
    }
}