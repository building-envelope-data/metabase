using System;
using Icon.Infrastructure.Aggregate;
using DateInterval = NodaTime.DateInterval;
using Marten.Schema;

namespace Icon.Domain
{
    public sealed class ComponentVersionOwnershipAggregate : EventSourcedAggregate
    {
        [ForeignKey(typeof(ComponentVersionAggregate))]
        public Guid ComponentVersionId { get; private set; }

        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Abbreviation { get; private set; }
        public DateInterval Availability { get; private set; }

        public static ComponentVersionOwnershipAggregate Create(ComponentVersionOwnership.Create.Event @event)
        {
            var ownership = new ComponentVersionOwnershipAggregate();
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
            Name = @event.Data.Name;
            Description = @event.Data.Description;
            Abbreviation = @event.Data.Abbreviation;
            Availability = @event.Data.Availability;
            Version++; // Ensure to update version on every Apply method.
        }
    }
}