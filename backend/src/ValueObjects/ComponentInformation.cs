using System.Collections.Generic;
using Errors = Icon.Errors;
using Guid = System.Guid;
using DateTime = System.DateTime;
using CSharpFunctionalExtensions;
using IError = HotChocolate.IError;
using Array = System.Array;

namespace Icon.ValueObjects
{
    public sealed class ComponentInformation
      : ValueObject
    {
        public Name Name { get; }
        public Abbreviation? Abbreviation { get; }
        public Description Description { get; }
        public DateInterval? Availability { get; }
        public IReadOnlyCollection<ComponentCategory> Categories { get; }

        private ComponentInformation(
            Name name,
            Abbreviation? abbreviation,
            Description description,
            DateInterval? availability,
            IReadOnlyCollection<ComponentCategory> categories
            )
        {
            Name = name;
            Abbreviation = abbreviation;
            Description = description;
            Availability = availability;
            Categories = categories;
        }

        public static Result<ComponentInformation, Errors> From(
            Name name,
            Abbreviation? abbreviation,
            Description description,
            DateInterval? availability,
            IReadOnlyCollection<ComponentCategory> categories,
            IReadOnlyList<object>? path = null
            )
        {
            return Result.Ok<ComponentInformation, Errors>(
                new ComponentInformation(
                  name: name,
                  abbreviation: abbreviation,
                  description: description,
                  availability: availability,
                  categories: categories
                  )
                );
        }

        public static Result<ComponentInformation, Errors> From(
            string name,
            string? abbreviation,
            string description,
            DateTime? availableFrom,
            DateTime? availableUntil,
            IReadOnlyCollection<ComponentCategory> categories,
            IReadOnlyList<object>? path = null
            )
        {
            var nameResult = ValueObjects.Name.From(name);
            var abbreviationResult = ValueObjects.Abbreviation.MaybeFrom(abbreviation);
            var descriptionResult = ValueObjects.Description.From(description);
            var availabilityResult = ValueObjects.DateInterval.MaybeFrom(availableFrom, availableUntil);

            return
              Errors.CombineExistent(
                  nameResult,
                  abbreviationResult,
                  descriptionResult,
                  availabilityResult
                  )
              .Bind(_ =>
                  From(
                    name: nameResult.Value,
                    abbreviation: abbreviationResult?.Value,
                    description: descriptionResult.Value,
                    availability: availabilityResult?.Value,
                    categories: categories,
                    path: path
                    )
                  );
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Name;
            yield return Abbreviation;
            yield return Description;
            yield return Availability;
            foreach (var category in Categories)
            {
                yield return category;
            }
        }
    }
}