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

        private void Apply(Marten.Events.Event<Events.ComponentVersionCreated> @event)
        {
            var data = @event.Data;
            Id = data.ComponentVersionId;
            ComponentId = data.ComponentId;
            Timestamp = @event.Timestamp.UtcDateTime;
            Version = @event.Version;
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