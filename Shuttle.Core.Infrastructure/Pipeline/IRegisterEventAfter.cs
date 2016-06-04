namespace Shuttle.Core.Infrastructure
{
    public interface IRegisterEventAfter
    {
        IPipelineStage Register<TPipelineEvent>() where TPipelineEvent : IPipelineEvent, new();
        IPipelineStage Register(IPipelineEvent pipelineEventToRegister);
    }
}