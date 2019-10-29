using System;
using Icon.Infrastructure.Aggregate;
using Marten.Schema;
/* using DateInterval = NodaTime.DateInterval; */
using DateTime = System.DateTime;

namespace Icon.Domain
{
    public sealed class ComponentVersionOwnershipAggregate : EventSourcedAggregate
    {
        [ForeignKey(typeof(ComponentVersionAggregate))]
        public Guid ComponentVersionId { get; private set; }

        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Abbreviation { get; private set; }
        /* public DateInterval Availability { get; private set; } */
        public DateTime AvailableFrom { get; set; } // TODO We only want a date here without time, but such a type does not exist in ASP.NET. We should use `NodaTime` but that is incompatible with ASP.NET `Identity` at the moment.
        public DateTime AvailableUntil { get; set; } // TODO We only want a date here without time, but such a type does not exist in ASP.NET. We should use `NodaTime` but that is incompatible with ASP.NET `Identity` at the moment.

        public static ComponentVersionOwnershipAggregate Create(ComponentVersionOwnership.Create.ComponentVersionOwnershipEvent @event)
        {
            var ownership = new ComponentVersionOwnershipAggregate();
            ownership.Apply(@event);
            ownership.AppendUncommittedEvent(@event);
            return ownership;
        }

        public ComponentVersionOwnershipAggregate()
        {
        }

        private void Apply(ComponentVersionOwnership.Create.ComponentVersionOwnershipEvent @event)
        {
            Id = @event.ComponentVersionOwnershipId;
            ComponentVersionId = @event.ComponentVersionId;
            Name = @event.Data.Name;
            Description = @event.Data.Description;
            Abbreviation = @event.Data.Abbreviation;
            /* Availability = @event.Data.Availability; */
            AvailableFrom = @event.Data.AvailableFrom;
            AvailableUntil = @event.Data.AvailableUntil;
            Version++; // Ensure to update version on every Apply method.
        }
    }
}