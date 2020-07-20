using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Metabase.ValueObjects
{
    public sealed class CreateMethodInput
      : ValueObject
    {
        public Name Name { get; }
        public Description Description { get; }
        public Id? StandardId { get; }
        public AbsoluteUri? PublicationLocator { get; }
        public AbsoluteUri? CodeLocator { get; }
        public IReadOnlyCollection<MethodCategory> Categories { get; }

        private CreateMethodInput(
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

        public static Result<CreateMethodInput, Errors> From(
            Name name,
            Description description,
            Id? standardId,
            AbsoluteUri? publicationLocator,
            AbsoluteUri? codeLocator,
            IReadOnlyCollection<MethodCategory> categories,
            IReadOnlyList<object>? path = null
            )
        {
            return Result.Success<CreateMethodInput, Errors>(
                new CreateMethodInput(
                  name: name,
                  description: description,
                  standardId: standardId,
                  publicationLocator: publicationLocator,
                  codeLocator: codeLocator,
                  categories: categories
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