using System.Collections.Generic;
using Errors = Icon.Errors;
using Validatable = Icon.Validatable;
using Uri = System.Uri;
using Guid = System.Guid;
using DateTime = System.DateTime;
using CSharpFunctionalExtensions;
using IError = HotChocolate.IError;
using Array = System.Array;

namespace Icon.ValueObjects
{
    public class MethodInformation
      : ValueObject
    {
        public Name Name { get; }
        public Description Description { get; }
        public Id? StandardId { get; }
        public AbsoluteUri? PublicationLocator { get; }
        public AbsoluteUri? CodeLocator { get; }
        public IReadOnlyCollection<MethodCategory> Categories { get; }

        private MethodInformation(
            Name name,
            Description description,
            Id? standardId,
            AbsoluteUri? publicationLocator,
            AbsoluteUri? codeLocator,
            IReadOnlyCollection<MethodCategory> categories
            )
        {
            Name = name;
            Description = description;
            StandardId = standardId;
            PublicationLocator = publicationLocator;
            CodeLocator = codeLocator;
            Categories = categories;
        }

        public static Result<MethodInformation, Errors> From(
            Name name,
            Description description,
            Id? standardId,
            AbsoluteUri? publicationLocator,
            AbsoluteUri? codeLocator,
            IReadOnlyCollection<MethodCategory> categories,
            IReadOnlyList<object>? path = null
            )
        {
            return Result.Ok<MethodInformation, Errors>(
                new MethodInformation(
                  name: name,
                  description: description,
                  standardId: standardId,
                  publicationLocator: publicationLocator,
                  codeLocator: codeLocator,
                  categories: categories
                  )
                );
        }

        public static Result<MethodInformation, Errors> From(
            string name,
            string description,
            Guid? standardId,
            Uri? publicationLocator,
            Uri? codeLocator,
            IReadOnlyCollection<MethodCategory> categories,
            IReadOnlyList<object>? path = null
            )
        {
            var nameResult = ValueObjects.Name.From(name);
            var descriptionResult = ValueObjects.Description.From(description);
            var standardIdResult = ValueObjects.Id.MaybeFrom(standardId);
            var publicationLocatorResult = ValueObjects.AbsoluteUri.MaybeFrom(publicationLocator);
            var codeLocatorResult = ValueObjects.AbsoluteUri.MaybeFrom(codeLocator);

            return
              Errors.CombineExistent(
                  nameResult,
                  descriptionResult,
                  standardIdResult,
                  publicationLocatorResult,
                  codeLocatorResult
                  )
              .Bind(_ =>
                  From(
                    name: nameResult.Value,
                    description: descriptionResult.Value,
                    standardId: standardIdResult?.Value,
                    publicationLocator: publicationLocatorResult?.Value,
                    codeLocator: codeLocatorResult?.Value,
                    categories: categories,
                    path: path
                    )
                  );
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Name;
            yield return Description;
            yield return StandardId;
            yield return PublicationLocator;
            yield return CodeLocator;
            foreach (var category in Categories)
            {
                yield return category;
            }
        }
    }
}