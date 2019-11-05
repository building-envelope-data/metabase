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

        public static ComponentVersionAggregate Create(Events.ComponentVersionCreated @event)
        {
            var version = new ComponentVersionAggregate();
            version.Apply(@event);
            version.AppendUncommittedEvent(@event);
            return version;
        }

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
          if (Version == 0) return null;
            return new Models.ComponentVersion(
              id: Id,
              componentId: ComponentId,
              version: Version
            );
        }
    }
}