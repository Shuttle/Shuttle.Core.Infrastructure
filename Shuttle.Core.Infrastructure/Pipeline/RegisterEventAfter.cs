using System.Collections.Generic;

namespace Shuttle.Core.Infrastructure
{
    public class RegisterEventAfter : IRegisterEventAfter
    {
        private readonly List<IPipelineEvent> _eventsToExecute;
        private readonly IPipelineEvent _pipelineEvent;
        private readonly IPipelineStage _pipelineStage;

        public RegisterEventAfter(IPipelineStage pipelineStage, List<IPipelineEvent> eventsToExecute,
            IPipelineEvent pipelineEvent)
        {
            _pipelineStage = pipelineStage;
            _eventsToExecute = eventsToExecute;
            _pipelineEvent = pipelineEvent;
        }

        public IPipelineStage Register<TPipelineEvent>() where TPipelineEvent : IPipelineEvent, new()
        {
            return Register(new TPipelineEvent());
        }

        public IPipelineStage Register(IPipelineEvent pipelineEventToRegister)
        {
            Guard.AgainstNull(pipelineEventToRegister, "pipelineEventToRegister");

            var index = _eventsToExecute.IndexOf(_pipelineEvent);

            _eventsToExecute.Insert(index + 1, pipelineEventToRegister);

            return _pipelineStage;
        }
    }
}