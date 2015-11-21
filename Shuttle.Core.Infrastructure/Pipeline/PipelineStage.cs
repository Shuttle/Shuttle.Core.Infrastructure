using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Shuttle.Core.Infrastructure
{
    public class PipelineStage
    {
        protected readonly List<PipelineEvent> PipelineEvents = new List<PipelineEvent>();

        public PipelineStage(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }

        public IEnumerable<PipelineEvent> Events
        {
            get { return new ReadOnlyCollection<PipelineEvent>(PipelineEvents); }
        }

        public PipelineStage WithEvent<TPipelineEvent>() where TPipelineEvent : PipelineEvent, new()
        {
            return WithEvent(new TPipelineEvent());
        }

        public PipelineStage WithEvent(PipelineEvent pipelineEvent)
        {
            Guard.AgainstNull(pipelineEvent, "pipelineEvent");

            PipelineEvents.Add(pipelineEvent);

            return this;
        }

        public RegisterEventBefore BeforeEvent<TPipelineEvent>() where TPipelineEvent : PipelineEvent, new()
        {
            var eventName = typeof (TPipelineEvent).FullName;
            var pipelineEvent = PipelineEvents.Find(e => e.Name.Equals(eventName));

            if (pipelineEvent == null)
            {
                throw new InvalidOperationException(string.Format(InfrastructureResources.PipelineStageEventNotRegistered,
                    Name, eventName));
            }

            return new RegisterEventBefore(PipelineEvents, pipelineEvent);
        }

        public RegisterEventAfter AfterEvent<TPipelineEvent>() where TPipelineEvent : PipelineEvent, new()
        {
            var eventName = typeof (TPipelineEvent).FullName;
            var pipelineEvent = PipelineEvents.Find(e => e.Name.Equals(eventName));

            if (pipelineEvent == null)
            {
                throw new InvalidOperationException(string.Format(InfrastructureResources.PipelineStageEventNotRegistered,
                    Name, eventName));
            }

            return new RegisterEventAfter(this, PipelineEvents, pipelineEvent);
        }
    }
}