// Inspired by https://jasperfx.github.io/marten/documentation/scenarios/aggregates_events_repositories/

using System;
using Icon.Infrastructure.Aggregate;
using Icon.Domain;

namespace Icon.Domain
{
    public class ComponentAggregate : EventSourcedAggregate
    {
        public static ComponentAggregate Create(Guid creatorId)
        {
            var component = new ComponentAggregate();
            var @event = new Component.Create.Event
            {
                ComponentId = Guid.NewGuid(),
                CreatorId = creatorId,
            };
            component.Apply(@event);
            component.AppendUncommittedEvent(@event);
            return component;
        }

        public ComponentAggregate()
        {
        }

        private void Apply(Component.Create.Event @event)
        {
            Id = @event.ComponentId;
            Version++; // Ensure to update version on every Apply method.
        }
    }
}