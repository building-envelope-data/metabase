using System.Collections.Generic;
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
              input.Name,
              path.Append("name").ToList().AsReadOnly()
              );
          var abbreviationResult = ValueObjects.Abbreviation.From(
              input.Abbreviation,
              path.Append("abbreviation").ToList().AsReadOnly()
              );
          var descriptionResult = ValueObjects.Description.From(
              input.Description,
              path.Append("description").ToList().AsReadOnly()
              );
          var availabilityResult = ValueObjects.Availability.From(
              input.AvailableFrom,
              input.AvailableUntil,
              path.Append("availableUntil").ToList().AsReadOnly()
              );

					var errors = Errors.From(
							nameResult,
							abbreviationResult,
							descriptionResult,
							availabilityResult
							);

          if (!errors.IsEmpty())
						return Result.Failure<ValueObjects.ComponentInput, Errors>(errors);

					return ValueObjects.ComponentInput.From(
								name: nameResult.Value,
								abbreviation: abbreviationResult.Value,
								description: descriptionResult.Value,
								availability: availabilityResult.Value,
								categories: Categories
								);
				}
		}
}