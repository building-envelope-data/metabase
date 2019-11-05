using System;
using Icon.Infrastructure.Aggregate;
using Marten.Schema;
/* using DateInterval = NodaTime.DateInterval; */
using DateTime = System.DateTime;
using Events = Icon.Events;

namespace Icon.Aggregates
{
    public sealed class ComponentVersionManufacturerAggregate : EventSourcedAggregate
    {
        [ForeignKey(typeof(ComponentVersionAggregate))]
        public Guid ComponentVersionId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string Abbreviation { get; set; }
        /* public DateInterval Availability { get; set; } */
        public DateTime AvailableFrom { get; set; } // TODO We only want a date here without time, but such a type does not exist in ASP.NET. We should use `NodaTime` but that is incompatible with ASP.NET `Identity` at the moment.
        public DateTime AvailableUntil { get; set; } // TODO We only want a date here without time, but such a type does not exist in ASP.NET. We should use `NodaTime` but that is incompatible with ASP.NET `Identity` at the moment.

        public static ComponentVersionManufacturerAggregate Create(Events.ComponentVersionManufacturerCreated @event)
        {
            var ownership = new ComponentVersionManufacturerAggregate();
            ownership.Apply(@event);
            ownership.AppendUncommittedEvent(@event);
            return ownership;
        }

        public ComponentVersionManufacturerAggregate()
        {
        }

        private void Apply(Events.ComponentVersionManufacturerCreated @event)
        {
            Id = @event.ComponentVersionManufacturerId;
            ComponentVersionId = @event.ComponentVersionId;
            Name = @event.Name;
            Description = @event.Description;
            Abbreviation = @event.Abbreviation;
            /* Availability = @event.Availability; */
            AvailableFrom = @event.AvailableFrom;
            AvailableUntil = @event.AvailableUntil;
            Version++; // Ensure to update version on every Apply method.
        }

        public Models.ComponentVersionManufacturer ToModel()
        {
            if (Version == 0) return null;
            return new Models.ComponentVersionManufacturer(
              id: Id,
              componentVersionId: ComponentVersionId,
              name: Name,
              description: Description,
              abbreviation: Abbreviation,
              availableFrom: AvailableFrom,
              availableUntil: AvailableUntil,
              version: Version
            );
        }
    }
}