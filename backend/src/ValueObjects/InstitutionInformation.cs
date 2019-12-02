using System.Collections.Generic;
using Errors = Icon.Errors;
using Uri = System.Uri;
using Guid = System.Guid;
using DateTime = System.DateTime;
using CSharpFunctionalExtensions;
using IError = HotChocolate.IError;
using Array = System.Array;

namespace Icon.ValueObjects
{
    public sealed class InstitutionInformation
      : ValueObject
    {
        public Name Name { get; }
        public Abbreviation? Abbreviation { get; }
        public Description? Description { get; }
        public AbsoluteUri? WebsiteLocator { get; }

        private InstitutionInformation(
            Name name,
            Abbreviation? abbreviation,
            Description? description,
            AbsoluteUri? websiteLocator
            )
        {
            Name = name;
            Abbreviation = abbreviation;
            Description = description;
            WebsiteLocator = websiteLocator;
        }

        public static Result<InstitutionInformation, Errors> From(
            Name name,
            Abbreviation? abbreviation,
            Description? description,
            AbsoluteUri? websiteLocator,
            IReadOnlyList<object>? path = null
            )
        {
            return Result.Ok<InstitutionInformation, Errors>(
                new InstitutionInformation(
                  name: name,
                  abbreviation: abbreviation,
                  description: description,
                  websiteLocator: websiteLocator
                  )
                );
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Name;
            yield return Abbreviation;
            yield return Description;
            yield return WebsiteLocator;
        }
    }
}