using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Shuttle.Core.Infrastructure
{
    public class PipelineStage : IPipelineStage
    {
        protected readonly List<IPipelineEvent> PipelineEvents = new List<IPipelineEvent>();

        public PipelineStage(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public IEnumerable<IPipelineEvent> Events => new ReadOnlyCollection<IPipelineEvent>(PipelineEvents);

        public IPipelineStage WithEvent<TPipelineEvent>() where TPipelineEvent : IPipelineEvent, new()
        {
            return WithEvent(new TPipelineEvent());
        }

        public IPipelineStage WithEvent(IPipelineEvent pipelineEvent)
        {
            Guard.AgainstNull(pipelineEvent, "pipelineEvent");

            PipelineEvents.Add(pipelineEvent);

            return this;
        }

        public IRegisterEventBefore BeforeEvent<TPipelineEvent>() where TPipelineEvent : IPipelineEvent, new()
        {
            var eventName = typeof(TPipelineEvent).FullName;
            var pipelineEvent = PipelineEvents.Find(e => e.Name.Equals(eventName));

            if (pipelineEvent == null)
            {
                throw new InvalidOperationException(
                    string.Format(InfrastructureResources.PipelineStageEventNotRegistered,
                        Name, eventName));
            }

            return new RegisterEventBefore(PipelineEvents, pipelineEvent);
        }

        public IRegisterEventAfter AfterEvent<TPipelineEvent>() where TPipelineEvent : IPipelineEvent, new()
        {
            var eventName = typeof(TPipelineEvent).FullName;
            var pipelineEvent = PipelineEvents.Find(e => e.Name.Equals(eventName));

            if (pipelineEvent == null)
            {
                throw new InvalidOperationException(
                    string.Format(InfrastructureResources.PipelineStageEventNotRegistered,
                        Name, eventName));
            }

            return new RegisterEventAfter(this, PipelineEvents, pipelineEvent);
        }
    }
}