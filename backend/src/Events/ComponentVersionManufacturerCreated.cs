using Guid = System.Guid;
using System.Threading.Tasks;
using CancellationToken = System.Threading.CancellationToken;
using Icon.Infrastructure;
using Icon.Infrastructure.Command;
using Icon.Infrastructure.Event;
using Icon.Infrastructure.Aggregate;
using Commands = Icon.Commands;
/* using DateInterval = NodaTime.DateInterval; */
using DateTime = System.DateTime;

namespace Icon.Events
{
    public sealed class ComponentVersionManufacturerCreated : EventBase
    {
        public Guid ComponentVersionManufacturerId { get; set; }
        public Guid ComponentVersionId { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Abbreviation { get; set; }
        /* public DateInterval Availability { get; set; } */ // TODO This is what we actually want, a proper date interval and it should be persisted as PostgreSQL date range
        public DateTime AvailableFrom { get; set; } // TODO We only want a date here without time, but such a type does not exist in ASP.NET. We should use `NodaTime` but that is incompatible with ASP.NET `Identity` at the moment.
        public DateTime AvailableUntil { get; set; } // TODO We only want a date here without time, but such a type does not exist in ASP.NET. We should use `NodaTime` but that is incompatible with ASP.NET `Identity` at the moment.

        public ComponentVersionManufacturerCreated() { }

        public ComponentVersionManufacturerCreated(Guid componentVersionManufacturerId, Commands.CreateComponentVersionManufacturer command) : base(command.CreatorId)
        {
            ComponentVersionManufacturerId = componentVersionManufacturerId;
            ComponentVersionId = command.ComponentVersionId;
            UserId = command.UserId;
            Name = command.Name;
            Description = command.Description;
            Abbreviation = command.Abbreviation;
            AvailableFrom = command.AvailableFrom;
            AvailableUntil = command.AvailableUntil;
        }
    }
}
