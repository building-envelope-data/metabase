using System.Collections.Generic;
using Guid = System.Guid;
using DateTime = System.DateTime;
/* using DateInterval = NodaTime.DateInterval; */

namespace Icon.Models
{
    public sealed class ComponentInformation
    {
        public string Name { get; }
        public string Abbreviation { get; }
        public string Description { get; }
        /* public DateInterval? Availability { get; } */ // TODO This is what we actually want, a proper date interval and it should be persisted as PostgreSQL date range
        public DateTime? AvailableFrom { get; } // TODO We only want a date here without time, but such a type does not exist in ASP.NET. We should use `NodaTime` but that is incompatible with ASP.NET `Identity` at the moment.
        public DateTime? AvailableUntil { get; } // TODO We only want a date here without time, but such a type does not exist in ASP.NET. We should use `NodaTime` but that is incompatible with ASP.NET `Identity` at the moment.
        public IEnumerable<ComponentCategory> Categories { get; }

        public ComponentInformation(
            string name,
            string abbreviation,
            string description,
            DateTime? availableFrom,
            DateTime? availableUntil,
            IEnumerable<ComponentCategory> categories
            )
        {
            Name = name;
            Abbreviation = abbreviation;
            Description = description;
            AvailableFrom = availableFrom;
            AvailableUntil = availableUntil;
            Categories = categories;
        }
    }
}