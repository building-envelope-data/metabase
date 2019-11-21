// Inspired by https://jasperfx.github.io/marten/documentation/scenarios/aggregates_events_repositories/

using System;
using System.Collections.Generic;
using Icon.Events;
using Newtonsoft.Json;

namespace Icon.Infrastructure.Aggregate
{
    public abstract class EventSourcedAggregate : Validatable, IEventSourcedAggregate
    {
        // For indexing our event streams
        public Guid Id { get; set; }

        public DateTime Timestamp { get; set; }

        // For protecting the state, i.e. conflict prevention
        public int Version { get; set; }

        protected EventSourcedAggregate()
        {
            Id = Guid.Empty;
            Timestamp = DateTime.MinValue;
            Version = 0;
        }

        protected void ApplyMeta<E>(Marten.Events.Event<E> @event)
          where E : IEvent
        {
            @event.Data.EnsureValid();
            Timestamp = @event.Timestamp.UtcDateTime;
            Version = @event.Version;
        }

        public bool IsVirgin()
        {
            return
               Id == Guid.Empty &&
               Timestamp == DateTime.MinValue &&
               Version is 0;
        }

        public override bool IsValid()
        {
            return IsVirgin() || (
               Id != Guid.Empty &&
               Timestamp != DateTime.MinValue &&
               !(Version is 0)
               );
        }
    }
}