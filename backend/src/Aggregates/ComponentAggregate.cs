// Inspired by https://jasperfx.github.io/marten/documentation/scenarios/aggregates_events_repositories/

using System;
using Icon.Infrastructure.Aggregate;
using Events = Icon.Events;

namespace Icon.Aggregates
{
    public sealed class ComponentAggregate : EventSourcedAggregate
    {
        public ComponentAggregate()
        {
        }

        private void Apply(Marten.Events.Event<Events.ComponentCreated> @event)
        {
            var data = @event.Data;
            Id = data.ComponentId;
            Timestamp = @event.Timestamp.UtcDateTime;
            Version = @event.Version;
        }

        public Models.Component ToModel()
        {
            return new Models.Component(
                id: Id,
                timestamp: Timestamp
                );
        }
    }
}