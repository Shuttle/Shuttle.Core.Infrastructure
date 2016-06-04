using System.Collections.Generic;

namespace Shuttle.Core.Infrastructure
{
    public class RegisterEventBefore : IRegisterEventBefore
    {
        private readonly List<IPipelineEvent> _eventsToExecute;
        private readonly IPipelineEvent _pipelineEvent;

        public RegisterEventBefore(List<IPipelineEvent> eventsToExecute, IPipelineEvent pipelineEvent)
        {
            _eventsToExecute = eventsToExecute;
            _pipelineEvent = pipelineEvent;
        }

        public void Register<TPipelineEvent>() where TPipelineEvent : IPipelineEvent, new()
        {
            Register(new TPipelineEvent());
        }

        public void Register(IPipelineEvent pipelineEventToRegister)
        {
            Guard.AgainstNull(pipelineEventToRegister, "pipelineEventToRegister");

            var index = _eventsToExecute.IndexOf(_pipelineEvent);

            _eventsToExecute.Insert(index, pipelineEventToRegister);
        }
    }
}