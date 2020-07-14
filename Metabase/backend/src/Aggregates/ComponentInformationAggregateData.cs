using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using DateTime = System.DateTime;
using Errors = Infrastructure.Errors;
using Validatable = Infrastructure.Validatable;

namespace Metabase.Aggregates
{
    public sealed class ComponentInformationAggregateData
      : Validatable
    {
        public static ComponentInformationAggregateData From(
            Events.ComponentInformationEventData information
            )
        {
            return new ComponentInformationAggregateData(
                name: information.Name,
                abbreviation: information.Abbreviation,
                description: information.Description,
                availableFrom: information.AvailableFrom,
                availableUntil: information.AvailableUntil,
                categories: information.Categories
                .Select(Events.ComponentCategoryEventDataExtensions.ToModel)
                .ToList()
                );
        }

        public string Name { get; set; }
        public string? Abbreviation { get; set; }
        public string Description { get; set; }
        public DateTime? AvailableFrom { get; set; }
        public DateTime? AvailableUntil { get; set; }
        public ICollection<ValueObjects.ComponentCategory> Categories { get; set; }

#nullable disable
        public ComponentInformationAggregateData() { }
#nullable enable

        public ComponentInformationAggregateData(
            string name,
            string? abbreviation,
            string description,
            DateTime? availableFrom,
            DateTime? availableUntil,
            ICollection<ValueObjects.ComponentCategory> categories
            )
        {
            Name = name;
            Abbreviation = abbreviation;
            Description = description;
            AvailableFrom = availableFrom;
            AvailableUntil = availableUntil;
            Categories = categories;
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

        public Result<ValueObjects.ComponentInformation, Errors> ToValueObject()
        {
            return ValueObjects.ComponentInformation.From(
                name: Name,
                abbreviation: Abbreviation,
                description: Description,
                availableFrom: AvailableFrom,
                availableUntil: AvailableUntil,
                categories: Categories.ToList().AsReadOnly()
                );
        }
    }
}