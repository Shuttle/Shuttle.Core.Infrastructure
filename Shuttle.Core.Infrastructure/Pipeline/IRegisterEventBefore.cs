namespace Shuttle.Core.Infrastructure
{
    public interface IRegisterEventBefore
    {
        void Register<TPipelineEvent>() where TPipelineEvent : IPipelineEvent, new();
        void Register(IPipelineEvent pipelineEventToRegister);
    }
}