using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using DateTime = System.DateTime;
using Errors = Icon.Errors;
using Guid = System.Guid;
using Models = Icon.Models;

namespace Icon.Events
{
    public sealed class ComponentInformationEventData
      : Validatable
    {
        public static ComponentInformationEventData From(
            ValueObjects.ComponentInformation information
            )
        {
            return new ComponentInformationEventData(
                  name: information.Name,
                  abbreviation: information.Abbreviation?.Value,
                  description: information.Description,
                  availableFrom: information.Availability?.Start,
                  availableUntil: information.Availability?.End,
                  categories: information.Categories.Select(c => c.FromModel()).ToList().AsReadOnly()
                );
        }

        public string Name { get; set; }
        public string? Abbreviation { get; set; }
        public string Description { get; set; }
        /* public DateInterval? Availability { get; } */ // TODO This is what we actually want, a proper date interval and it should be persisted as PostgreSQL date range. This should work with `NodaTime.DateInterval`.
        public DateTime? AvailableFrom { get; set; }
        public DateTime? AvailableUntil { get; set; }
        public IReadOnlyCollection<ComponentCategoryEventData> Categories { get; set; }

#nullable disable
        public ComponentInformationEventData() { }
#nullable enable

        public ComponentInformationEventData(
            string name,
            string? abbreviation,
            string description,
            DateTime? availableFrom,
            DateTime? availableUntil,
            IReadOnlyCollection<ComponentCategoryEventData> categories
            )
        {
            Name = name;
            Abbreviation = abbreviation;
            Description = description;
            AvailableFrom = availableFrom;
            AvailableUntil = availableUntil;
            Categories = categories;
            EnsureValid();
        }

        public override Result<bool, Errors> Validate()
        {
            return
              Result.Combine(
                  ValidateNonNull(Name, nameof(Name)),
                  ValidateNonNull(Description, nameof(Description)),
                  ValidateNonNull(Categories, nameof(Categories))
                  );
        }
    }
}