using System.Collections.Generic;
using Guid = System.Guid;
using DateTime = System.DateTime;
using Models = Icon.Models;
using System.Linq;
using Errors = Icon.Errors;
using CSharpFunctionalExtensions;

namespace Icon.Events
{
    public sealed class ComponentInformationEventData
      : Validatable
    {
        public static ComponentInformationEventData From(
            ValueObjects.ComponentInput input
            )
        {
            return new ComponentInformationEventData(
                  name: input.Name,
                  abbreviation: input.Abbreviation,
                  description: input.Description,
                  availableFrom: input.Availability?.Start,
                  availableUntil: input.Availability?.End,
                  categories: input.Categories.Select(c => c.FromModel()).ToList()
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
                base.Validate(),
                ValidateNonNull(Name, nameof(Name)),
                ValidateNonNull(Description, nameof(Description)),
                ValidateNonNull(Categories, nameof(Categories))
                );
        }
    }
}