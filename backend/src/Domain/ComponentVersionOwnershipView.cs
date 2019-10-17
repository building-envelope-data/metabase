using DateInterval = NodaTime.DateInterval;
using Guid = System.Guid;
using DateTime = System.DateTime;

namespace Icon.Domain
{
    public sealed class ComponentVersionOwnershipView
    {
        public Guid Id { get; set; }
        public Guid ComponentVersionId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Abbreviation { get; set; }
        /* public DateInterval Availability { get; set; } */
        public DateTime AvailableFrom { get; set; } // TODO We only want a date here without time, but such a type does not exist in ASP.NET. We should use `NodaTime` but that is incompatible with ASP.NET `Identity` at the moment.
        public DateTime AvailableUntil { get; set; } // TODO We only want a date here without time, but such a type does not exist in ASP.NET. We should use `NodaTime` but that is incompatible with ASP.NET `Identity` at the moment.
    }
}