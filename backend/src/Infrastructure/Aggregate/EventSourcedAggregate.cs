// Inspired by https://jasperfx.github.io/marten/documentation/scenarios/aggregates_events_repositories/

using System;
using System.Collections.Generic;
using Icon.Infrastructure.Event;
using Newtonsoft.Json;

namespace Icon.Infrastructure.Aggregate
{
    public abstract class EventSourcedAggregate : IEventSourcedAggregate
    {
        // For indexing our event streams
        public Guid Id { get; protected set; }

        // TODO Use attribute `[Version]`? See https://github.com/JasperFx/marten/blob/master/src/Marten/Schema/VersionAttribute.cs
        // For protecting the state, i.e. conflict prevention
        public int Version { get; protected set; }

        // JsonIgnore - for making sure that it won't be stored in inline projection
        [JsonIgnore]
        private readonly Queue<IEvent> _uncommittedEvents;

        protected EventSourcedAggregate()
        {
            _uncommittedEvents = new Queue<IEvent>();
            Version = 0;
        }

        public IEnumerable<IEvent> GetUncommittedEvents()
        {
            return _uncommittedEvents;
        }

        public void ClearUncommittedEvents()
        {
            _uncommittedEvents.Clear();
        }

        protected void AppendUncommittedEvent(IEvent @event)
        {
            _uncommittedEvents.Enqueue(@event);
        }
    }
}