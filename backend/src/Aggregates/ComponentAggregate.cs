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

        private void Apply(Events.ComponentCreated @event)
        {
            Id = @event.ComponentId;
            Version++; // Ensure to update version on every Apply method.
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