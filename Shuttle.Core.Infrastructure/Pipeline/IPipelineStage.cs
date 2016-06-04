using System.Collections.Generic;

namespace Shuttle.Core.Infrastructure
{
    public interface IPipelineStage
    {
        string Name { get; }
        IEnumerable<IPipelineEvent> Events { get; }
        IPipelineStage WithEvent<TPipelineEvent>() where TPipelineEvent : IPipelineEvent, new();
        IPipelineStage WithEvent(IPipelineEvent pipelineEvent);
        IRegisterEventBefore BeforeEvent<TPipelineEvent>() where TPipelineEvent : IPipelineEvent, new();
        IRegisterEventAfter AfterEvent<TPipelineEvent>() where TPipelineEvent : IPipelineEvent, new();
    }
}