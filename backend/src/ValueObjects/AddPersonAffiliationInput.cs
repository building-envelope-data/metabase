using System.Collections.Generic;
using Errors = Icon.Errors;
using CSharpFunctionalExtensions;
using DateTime = System.DateTime;

namespace Icon.ValueObjects
{
    public sealed class AddPersonAffiliationInput
      : ValueObject
    {
        public Id PersonId { get; }
        public Id InstitutionId { get; }

        private AddPersonAffiliationInput(
            Id personId,
            Id institutionId
            )
        {
            PersonId = personId;
            InstitutionId = institutionId;
        }

        public static Result<AddPersonAffiliationInput, Errors> From(
            Id personId,
            Id institutionId
            )
        {
            return
              Result.Ok<AddPersonAffiliationInput, Errors>(
                  new AddPersonAffiliationInput(
                    personId: personId,
                    institutionId: institutionId
                    )
                  );
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return PersonId;
            yield return InstitutionId;
        }
    }
}