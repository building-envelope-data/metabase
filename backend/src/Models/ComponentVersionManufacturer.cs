using Guid = System.Guid;
/* using DateInterval = NodaTime.DateInterval; */
using DateTime = System.DateTime;

namespace Icon.Models
{
  public sealed class ComponentVersionManufacturer
  {
    public Guid Id { get; }
    public Guid ComponentVersionId { get; }
    public string Name { get; }
    public string Description { get; }
    public string Abbreviation { get; }
    /* public DateInterval Availability { get; } */ // TODO This is what we actually want, a proper date interval and it should be persisted as PostgreSQL date range
    public DateTime AvailableFrom { get; } // TODO We only want a date here without time, but such a type does not exist in ASP.NET. We should use `NodaTime` but that is incompatible with ASP.NET `Identity` at the moment.
    public DateTime AvailableUntil { get; } // TODO We only want a date here without time, but such a type does not exist in ASP.NET. We should use `NodaTime` but that is incompatible with ASP.NET `Identity` at the moment.
    public int Version { get; }

    public ComponentVersionManufacturer(
        Guid id,
        Guid componentVersionId,
        string name,
        string description,
        string abbreviation,
        /* public DateInterval Availability { get; set; } */ // TODO This is what we actually want, a proper date interval and it should be persisted as PostgreSQL date range
        DateTime availableFrom,
        DateTime availableUntil,
        int version
        ) {
      Id = id;
      ComponentVersionId = componentVersionId;
      Name = name;
      Description = description;
      Abbreviation = abbreviation;
      AvailableFrom = availableFrom;
      AvailableUntil = availableUntil;
    }
  }
}
