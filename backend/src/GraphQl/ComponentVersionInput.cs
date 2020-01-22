using System.Collections.Generic;
using Guid = System.Guid;
using DateTime = System.DateTime;
using Models = Icon.Models;
using ValueObjects = Icon.ValueObjects;
using CSharpFunctionalExtensions;
using System.Linq;

namespace Icon.GraphQl
{
    public sealed class ComponentVersionInput
    {
        public Guid ComponentId { get; }
        public string Name { get; }
        public string? Abbreviation { get; }
        public string Description { get; }
        public DateTime? AvailableFrom { get; }
        public DateTime? AvailableUntil { get; }
        public IReadOnlyCollection<ValueObjects.ComponentCategory> Categories { get; }

        public ComponentVersionInput(
        Guid componentId,
        string name,
        string? abbreviation,
        string description,
        DateTime? availableFrom,
        DateTime? availableUntil,
        IReadOnlyCollection<ValueObjects.ComponentCategory> categories
        ) {
        ComponentId = componentId;
        Name = name;
        Abbreviation = abbreviation;
        Description = description;
        AvailableFrom = availableFrom;
        AvailableUntil = availableUntil;
        Categories = categories;
        }

        public Result<ValueObjects.ComponentVersionInput, Errors> Validate(
                IReadOnlyList<object> path
                )
        {
            var componentIdResult = ValueObjects.Id.From(
                ComponentId,
                path.Append("componentId").ToList().AsReadOnly()
                );
            var nameResult = ValueObjects.Name.From(
                Name,
                path.Append("name").ToList().AsReadOnly()
                );
            var abbreviationResult = ValueObjects.Abbreviation.MaybeFrom(
                Abbreviation,
                path.Append("abbreviation").ToList().AsReadOnly()
                );
            var descriptionResult = ValueObjects.Description.From(
                Description,
                path.Append("description").ToList().AsReadOnly()
                );
            var availabilityResult = ValueObjects.DateInterval.MaybeFrom(
                AvailableFrom,
                AvailableUntil,
                path.Append("availableUntil").ToList().AsReadOnly()
                );

            return
    Errors.CombineExistent(
      componentIdResult,
                    nameResult,
                    abbreviationResult,
                    descriptionResult,
                    availabilityResult
                    )
    .Bind(_ =>
            ValueObjects.ComponentVersionInput.From(
        componentId: componentIdResult.Value,
                        name: nameResult.Value,
                        abbreviation: abbreviationResult?.Value,
                        description: descriptionResult.Value,
                        availability: availabilityResult?.Value,
                        categories: Categories
                        )
  );
        }
    }
}