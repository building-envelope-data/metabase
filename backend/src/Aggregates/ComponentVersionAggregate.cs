using System;
using Icon.Infrastructure.Aggregate;
using Marten.Schema;
using Events = Icon.Events;

namespace Icon.Aggregates
{
    public sealed class ComponentVersionAggregate
      : EventSourcedAggregate
    {
        [ForeignKey(typeof(ComponentAggregate))]
        public Guid ComponentId { get; set; }

        public ComponentVersionAggregate()
        {
        }

        private void Apply(Events.ComponentVersionCreated @event)
        {
            Id = @event.ComponentVersionId;
            ComponentId = @event.ComponentId;
            Version++; // Ensure to update version on every Apply method.
        }

        public Models.ComponentVersion ToModel()
        {
            return new Models.ComponentVersion(
              id: Id,
              componentId: ComponentId,
              timestamp: Timestamp
            );
        }
    }
}