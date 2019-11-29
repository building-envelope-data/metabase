using Console = System.Console;
using System.Collections.Generic;
using System.Linq;
using Array = System.Array;
using Guid = System.Guid;
using DateTime = System.DateTime;
using Models = Icon.Models;
using ValueObjects = Icon.ValueObjects;
using CSharpFunctionalExtensions;

namespace Icon.GraphQl
{
    public sealed class ComponentInput
    {
        public string Name { get; set; }
        public string? Abbreviation { get; set; }
        public string Description { get; set; }
        public DateTime? AvailableFrom { get; set; }
        public DateTime? AvailableUntil { get; set; }
        public IReadOnlyCollection<ValueObjects.ComponentCategory> Categories { get; set; }

#nullable disable
        public ComponentInput() { }
#nullable enable

        public Result<ValueObjects.ComponentInput, Errors> Validate(
            IReadOnlyList<object> path
            )
        {
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
                  nameResult,
                  abbreviationResult,
                  descriptionResult,
                  availabilityResult
                  )
              .Bind(_ =>
                  ValueObjects.ComponentInput.From(
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