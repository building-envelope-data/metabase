using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace Icon.ValueObjects
{
    public sealed class CreateInstitutionInput
      : ValueObject
    {
        public Name Name { get; }
        public Abbreviation? Abbreviation { get; }
        public Description? Description { get; }
        public AbsoluteUri? WebsiteLocator { get; }
        public PublicKey? PublicKey { get; }
        public InstitutionState State { get; }

        private CreateInstitutionInput(
            Name name,
            Abbreviation? abbreviation,
            Description? description,
            AbsoluteUri? websiteLocator,
            PublicKey? publicKey,
            InstitutionState state
            )
        {
            Name = name;
            Abbreviation = abbreviation;
            Description = description;
            WebsiteLocator = websiteLocator;
            PublicKey = publicKey;
            State = state;
        }

        public static Result<CreateInstitutionInput, Errors> From(
            Name name,
            Abbreviation? abbreviation,
            Description? description,
            AbsoluteUri? websiteLocator,
            PublicKey? publicKey,
            InstitutionState state,
            IReadOnlyList<object>? path = null
            )
        {
            return Result.Ok<CreateInstitutionInput, Errors>(
                new CreateInstitutionInput(
                  name: name,
                  abbreviation: abbreviation,
                  description: description,
                  websiteLocator: websiteLocator,
                  publicKey: publicKey,
                  state: state
                  )
                );
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Name;
            yield return Abbreviation;
            yield return Description;
            yield return WebsiteLocator;
            yield return PublicKey;
            yield return State;
        }
    }
}