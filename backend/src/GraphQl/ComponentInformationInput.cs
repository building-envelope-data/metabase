using System.Collections.Generic;
using System.Linq;
using Models = Icon.Models;
using DateTime = System.DateTime;
using CSharpFunctionalExtensions;

namespace Icon.GraphQl
{
    public sealed class ComponentInformationInput
    {
        public string Name { get; }
        public string? Abbreviation { get; }
        public string Description { get; }
        public DateTime? AvailableFrom { get; }
        public DateTime? AvailableUntil { get; }
        public IEnumerable<ValueObjects.ComponentCategory> Categories { get; }

        public ComponentInformationInput(
            string name,
            string? abbreviation,
            string description,
            DateTime? availableFrom,
            DateTime? availableUntil,
            IEnumerable<ValueObjects.ComponentCategory> categories
            )
        {
            Name = name;
            Abbreviation = abbreviation;
            Description = description;
            AvailableFrom = availableFrom;
            AvailableUntil = availableUntil;
            Categories = categories;
        }

        // TODO Figure out how methods can be made to be ignored by
        // HotChocolate and remove the keyword `static` that makes this method
        // a class method as opposed to an instance method.
        public static Result<ValueObjects.ComponentInformation, Errors> Validate(
            ComponentInformationInput self,
            IReadOnlyList<object> path
            )
        {
            var nameResult = ValueObjects.Name.From(
                self.Name,
                path.Append("name").ToList().AsReadOnly()
                );
            var abbreviationResult = ValueObjects.Abbreviation.MaybeFrom(
                self.Abbreviation,
                path.Append("abbreviation").ToList().AsReadOnly()
                );
            var descriptionResult = ValueObjects.Description.From(
                self.Description,
                path.Append("description").ToList().AsReadOnly()
                );
            var availabilityResult = ValueObjects.DateInterval.MaybeFrom(
                self.AvailableFrom,
                self.AvailableUntil,
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
                  ValueObjects.ComponentInformation.From(
                    name: nameResult.Value,
                    abbreviation: abbreviationResult?.Value,
                    description: descriptionResult.Value,
                    availability: availabilityResult?.Value,
                    categories: self.Categories.ToList().AsReadOnly()
                    )
                  );
        }
    }
}