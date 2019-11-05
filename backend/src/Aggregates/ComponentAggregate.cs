// Inspired by https://jasperfx.github.io/marten/documentation/scenarios/aggregates_events_repositories/

using System;
using Icon.Infrastructure.Aggregate;
using Events = Icon.Events;

namespace Icon.Aggregates
{
    public sealed class ComponentAggregate : EventSourcedAggregate
    {
        public static ComponentAggregate Create(Events.ComponentCreated @event)
        {
            var component = new ComponentAggregate();
            component.Apply(@event);
            component.AppendUncommittedEvent(@event);
            return component;
        }

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
            if (Version == 0) return null;
            return new Models.Component(
                id: Id,
                version: Version
                );
        }
    }
}