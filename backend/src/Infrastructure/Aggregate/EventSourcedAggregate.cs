// Inspired by https://jasperfx.github.io/marten/documentation/scenarios/aggregates_events_repositories/

using System;
using System.Collections.Generic;
using Icon.Events;
using Newtonsoft.Json;

namespace Icon.Infrastructure.Aggregate
{
    public abstract class EventSourcedAggregate : IEventSourcedAggregate
    {
        // For indexing our event streams
        public Guid Id { get; set; }

        public DateTime Timestamp { get; set; }

        // For protecting the state, i.e. conflict prevention
        public int Version { get; set; }

        protected EventSourcedAggregate() { }

        protected void ApplyMeta<E>(Marten.Events.Event<E> @event)
          where E : IEvent
        {
            @event.Data.EnsureValid();
            Timestamp = @event.Timestamp.UtcDateTime;
            Version = @event.Version;
        }
    }
}