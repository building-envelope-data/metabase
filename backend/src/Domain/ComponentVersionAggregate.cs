using System;
using Icon.Infrastructure.Aggregate;
using Marten.Schema;

namespace Icon.Domain
{
    public sealed class ComponentVersionAggregate : EventSourcedAggregate
    {
        [ForeignKey(typeof(ComponentAggregate))]
        public Guid ComponentId { get; private set; }

        public static ComponentVersionAggregate Create(ComponentVersion.Create.ComponentVersionCreateEvent @event)
        {
            var version = new ComponentVersionAggregate();
            version.Apply(@event);
            version.AppendUncommittedEvent(@event);
            return version;
        }

        public ComponentVersionAggregate()
        {
        }

        private void Apply(ComponentVersion.Create.ComponentVersionCreateEvent @event)
        {
            Id = @event.ComponentVersionId;
            ComponentId = @event.ComponentId;
            Version++; // Ensure to update version on every Apply method.
        }
    }
}