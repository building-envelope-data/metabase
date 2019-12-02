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
        public Guid ComponentId { get; set; }
        public string Name { get; set; }
        public string? Abbreviation { get; set; }
        public string Description { get; set; }
        public DateTime? AvailableFrom { get; set; }
        public DateTime? AvailableUntil { get; set; }
        public IReadOnlyCollection<ValueObjects.ComponentCategory> Categories { get; set; }

#nullable disable
        public ComponentVersionInput() { }
#nullable enable

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