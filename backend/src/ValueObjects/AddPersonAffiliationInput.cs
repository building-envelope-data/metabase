using System.Collections.Generic;
using CSharpFunctionalExtensions;
using DateTime = System.DateTime;
using Errors = Icon.Errors;

namespace Icon.ValueObjects
{
    public sealed class AddPersonAffiliationInput
      : AddManyToManyAssociationInput
    {
        public Id PersonId { get => ParentId; }
        public Id InstitutionId { get => AssociateId; }

        private AddPersonAffiliationInput(
            Id personId,
            Id institutionId
            )
          : base(
              parentId: personId,
              associateId: institutionId
              )
        {
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
    }
}