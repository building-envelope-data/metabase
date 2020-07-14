using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Metabase.ValueObjects
{
    public sealed class CreateDatabaseInput
      : ValueObject
    {
        public Name Name { get; }
        public Description Description { get; }
        public AbsoluteUri Locator { get; }
        public Id InstitutionId { get; }

        private CreateDatabaseInput(
            Name name,
            Description description,
            AbsoluteUri locator,
            Id institutionId
            )
        {
            Name = name;
            Description = description;
            Locator = locator;
            InstitutionId = institutionId;
        }

        public static Result<CreateDatabaseInput, Errors> From(
            Name name,
            Description description,
            AbsoluteUri locator,
            Id institutionId
            )
        {
            return
              Result.Ok<CreateDatabaseInput, Errors>(
                  new CreateDatabaseInput(
                    name: name,
                    description: description,
                    locator: locator,
                    institutionId: institutionId
                    )
                  );
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Name;
            yield return Description;
            yield return Locator;
            yield return InstitutionId;
        }
    }
}