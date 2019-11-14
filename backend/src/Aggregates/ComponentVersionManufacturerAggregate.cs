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

        public ComponentVersionManufacturerAggregate()
        {
        }

        private void Apply(Marten.Events.Event<Events.ComponentVersionManufacturerCreated> @event)
        {
            var data = @event.Data;
            Id = data.ComponentVersionManufacturerId;
            ComponentVersionId = data.ComponentVersionId;
            Name = data.Name;
            Description = data.Description;
            Abbreviation = data.Abbreviation;
            /* Availability = data.Availability; */
            AvailableFrom = data.AvailableFrom;
            AvailableUntil = data.AvailableUntil;
            Timestamp = @event.Timestamp.UtcDateTime;
            Version = @event.Version;
        }

        public Models.ComponentVersionManufacturer ToModel()
        {
            return new Models.ComponentVersionManufacturer(
              id: Id,
              componentVersionId: ComponentVersionId,
              name: Name,
              description: Description,
              abbreviation: Abbreviation,
              availableFrom: AvailableFrom,
              availableUntil: AvailableUntil,
              timestamp: Timestamp
            );
        }
    }
}