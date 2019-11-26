using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Array = System.Array;
using Guid = System.Guid;
using DateTime = System.DateTime;
using Models = Icon.Models;

namespace Icon.ValueObjects
{
    public sealed class ComponentInput
      : ValueObject
    {
        public Name Name { get; }
        public Abbreviation? Abbreviation { get; }
        public Description Description { get; }
        public DateInterval Availability { get; }
        public IReadOnlyCollection<ComponentCategory> Categories { get; }

        private ComponentInput(
            Name name,
            Abbreviation? abbreviation,
            Description description,
            DateInterval availability,
            IReadOnlyCollection<ComponentCategory> categories
            )
        {
          Name = name;
          Abbreviation = abbreviation;
          Description = description;
          Availability = availability;
          Categories = categories;
        }

        public static Result<ComponentInput, Errors> From(
            Name name,
            Abbreviation? abbreviation,
            Description description,
            DateInterval availability,
            IReadOnlyCollection<ComponentCategory> categories,
            IReadOnlyList<object>? path = null
            )
        {
          return Result.Ok<ComponentInput, Errors>(
              new ComponentInput(
                name: name,
                abbreviation: abbreviation,
                description: description,
                availability: availability,
                categories: categories
                )
              );
        }

        protected override IEnumerable<object> GetEqualityComponents()
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
