using Guid = System.Guid;
using System.Threading.Tasks;
using CancellationToken = System.Threading.CancellationToken;
using Icon.Infrastructure;
using Icon.Infrastructure.Command;
using Icon.Infrastructure.Event;
using Icon.Infrastructure.Aggregate;
/* using DateInterval = NodaTime.DateInterval; */
using DateTime = System.DateTime;
using IError = HotChocolate.IError;
using CSharpFunctionalExtensions;

namespace Icon.Commands
{
    public sealed class CreateComponentVersionManufacturer
      : CommandBase<Result<(Guid Id, DateTime Timestamp), IError>>
    {
        public Guid ComponentVersionId { get; }
        public Guid UserId { get; }
        public string Name { get; }
        public string Description { get; }
        public string Abbreviation { get; }
        /* public DateInterval Availability { get; } */ // TODO This is what we actually want, a proper date interval and it should be persisted as PostgreSQL date range
        public DateTime AvailableFrom { get; } // TODO We only want a date here without time, but such a type does not exist in ASP.NET. We should use `NodaTime` but that is incompatible with ASP.NET `Identity` at the moment.
        public DateTime AvailableUntil { get; } // TODO We only want a date here without time, but such a type does not exist in ASP.NET. We should use `NodaTime` but that is incompatible with ASP.NET `Identity` at the moment.

        public CreateComponentVersionManufacturer(
            Guid componentVersionId,
            Guid userId,
            string name,
            string description,
            string abbreviation,
            /* public DateInterval Availability { get; set; } */ // TODO This is what we actually want, a proper date interval and it should be persisted as PostgreSQL date range
            DateTime availableFrom,
            DateTime availableUntil,
            Guid creatorId
            ) : base(creatorId)
        {
            ComponentVersionId = componentVersionId;
            UserId = userId;
            Name = name;
            Description = description;
            Abbreviation = abbreviation;
            AvailableFrom = availableFrom;
            AvailableUntil = availableUntil;
        }
    }
}