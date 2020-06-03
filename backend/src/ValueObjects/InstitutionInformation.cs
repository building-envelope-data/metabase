using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Array = System.Array;
using DateTime = System.DateTime;
using Errors = Icon.Errors;
using Guid = System.Guid;
using IError = HotChocolate.IError;
using Uri = System.Uri;

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

        public static Result<InstitutionInformation, Errors> From(
            string name,
            string? abbreviation,
            string? description,
            Uri? websiteLocator,
            IReadOnlyList<object>? path = null
            )
        {
            var nameResult = ValueObjects.Name.From(name);
            var abbreviationResult = ValueObjects.Abbreviation.MaybeFrom(abbreviation);
            var descriptionResult = ValueObjects.Description.MaybeFrom(description);
            var websiteLocatorResult = ValueObjects.AbsoluteUri.MaybeFrom(websiteLocator);

            return
              Errors.CombineExistent(
                  nameResult,
                  abbreviationResult,
                  descriptionResult,
                  websiteLocatorResult
                  )
              .Bind(_ =>
                  ValueObjects.InstitutionInformation.From(
                    name: nameResult.Value,
                    abbreviation: abbreviationResult?.Value,
                    description: descriptionResult?.Value,
                    websiteLocator: websiteLocatorResult?.Value,
                    path: path
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