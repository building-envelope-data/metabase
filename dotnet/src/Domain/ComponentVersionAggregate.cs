using System;
using Icon.Infrastructure.Aggregate;

namespace Icon.Domain
{
    public class ComponentVersionAggregate : EventSourcedAggregate
    {
        private Guid ComponentId { get; set; }

        public static ComponentVersionAggregate Create(Guid componentId, Guid creatorId)
        {
            var version = new ComponentVersionAggregate();
            var @event = new ComponentVersion.Create.Event
            {
                ComponentVersionId = Guid.NewGuid(),
                ComponentId = componentId,
                CreatorId = creatorId,
            };
            version.Apply(@event);
            version.AppendUncommittedEvent(@event);
            return version;
        }

        public ComponentVersionAggregate()
        {
        }

        private void Apply(ComponentVersion.Create.Event @event)
        {
            Id = @event.ComponentVersionId;
            ComponentId = @event.ComponentId;
            Version++; // Ensure to update version on every Apply method.
        }
    }
}