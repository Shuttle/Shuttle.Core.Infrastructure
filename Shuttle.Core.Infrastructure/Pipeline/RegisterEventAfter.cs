using System.Collections.Generic;

namespace Shuttle.Core.Infrastructure
{
    public class RegisterEventAfter
    {
        private readonly List<PipelineEvent> eventsToExecute;
        private readonly PipelineEvent pipelineEvent;
        private readonly PipelineStage pipelineStage;

        public RegisterEventAfter(PipelineStage pipelineStage, List<PipelineEvent> eventsToExecute,
            PipelineEvent pipelineEvent)
        {
            this.pipelineStage = pipelineStage;
            this.eventsToExecute = eventsToExecute;
            this.pipelineEvent = pipelineEvent;
        }

        public PipelineStage Register<TPipelineEvent>() where TPipelineEvent : PipelineEvent, new()
        {
            return Register(new TPipelineEvent());
        }

        public PipelineStage Register(PipelineEvent pipelineEventToRegister)
        {
            Guard.AgainstNull(pipelineEventToRegister, "pipelineEventToRegister");

            var index = eventsToExecute.IndexOf(pipelineEvent);

            eventsToExecute.Insert(index + 1, pipelineEventToRegister);

            return pipelineStage;
        }
    }
}