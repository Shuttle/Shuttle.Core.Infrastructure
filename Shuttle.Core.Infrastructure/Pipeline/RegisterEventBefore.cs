using System.Collections.Generic;

namespace Shuttle.Core.Infrastructure
{
    public class RegisterEventBefore
    {
        private readonly List<PipelineEvent> eventsToExecute;
        private readonly PipelineEvent pipelineEvent;

        public RegisterEventBefore(List<PipelineEvent> eventsToExecute, PipelineEvent pipelineEvent)
        {
            this.eventsToExecute = eventsToExecute;
            this.pipelineEvent = pipelineEvent;
        }

        public void Register<TPipelineEvent>() where TPipelineEvent : PipelineEvent, new()
        {
            Register(new TPipelineEvent());
        }

        public void Register(PipelineEvent pipelineEventToRegister)
        {
            Guard.AgainstNull(pipelineEventToRegister, "pipelineEventToRegister");

            var index = eventsToExecute.IndexOf(pipelineEvent);

            eventsToExecute.Insert(index, pipelineEventToRegister);
        }
    }
}