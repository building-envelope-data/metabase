using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Guid = System.Guid;
using DateTime = System.DateTime;
using Models = Icon.Models;
using System.Linq;
using Events = Icon.Events;
using ValueObjects = Icon.ValueObjects;

namespace Icon.Aggregates
{
    public sealed class ComponentInformationAggregateData
      : Validatable
    {
        public static ComponentInformationAggregateData From(
            Events.ComponentInformationEventData information
            )
        {
            return new ComponentInformationAggregateData(
                name: information.Name.NotNull(),
                abbreviation: information.Abbreviation,
                description: information.Description.NotNull(),
                availableFrom: information.AvailableFrom,
                availableUntil: information.AvailableUntil,
                categories:
                information.Categories.NotNull()
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
          var nameResult = ValueObjects.Name.From(Name);
          var abbreviationResult = ValueObjects.Abbreviation.From(Abbreviation);
          var descriptionResult = ValueObjects.Description.From(Description);
          var availabilityResult = ValueObjects.DateInterval.From(AvailableFrom, AvailableUntil);

          var errors = Errors.From(
              nameResult,
              abbreviationResult,
              descriptionResult,
              availabilityResult
              );

          if (!errors.IsEmpty())
            return Result.Failure<ValueObjects.ComponentInformation, Errors>(errors);

          return ValueObjects.ComponentInformation.From(
              name: nameResult.Value,
              abbreviation: abbreviationResult.Value,
              description: descriptionResult.Value,
              availability: availabilityResult.Value,
              categories: Categories
              );
        }
    }
}