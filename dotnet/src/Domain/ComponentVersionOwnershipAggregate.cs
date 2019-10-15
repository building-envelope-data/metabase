using System;
using Icon.Infrastructure.Aggregate;

namespace Icon.Domain
{
    public class ComponentVersionOwnershipAggregate : EventSourcedAggregate
    {
        private Guid ComponentVersionId { get; set; }

        public static ComponentVersionOwnershipAggregate Create(Guid componentVersionId, Guid creatorId)
        {
            var ownership = new ComponentVersionOwnershipAggregate();
            var @event = new ComponentVersionOwnership.Create.Event
            {
                ComponentVersionOwnershipId = Guid.NewGuid(),
                ComponentVersionId = Guid.NewGuid(),
                CreatorId = creatorId,
            };
            ownership.Apply(@event);
            ownership.AppendUncommittedEvent(@event);
            return ownership;
        }

        public ComponentVersionOwnershipAggregate()
        {
        }

        private void Apply(ComponentVersionOwnership.Create.Event @event)
        {
            Id = @event.ComponentVersionOwnershipId;
            ComponentVersionId = @event.ComponentVersionId;
            Version++; // Ensure to update version on every Apply method.
        }
    }
}