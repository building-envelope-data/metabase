using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Errors = Icon.Errors;

namespace Icon.ValueObjects
{
    public sealed class AddInstitutionOperatedDatabaseInput
      : AddOneToManyAssociationInput
    {
        public static Result<AddInstitutionOperatedDatabaseInput, Errors> From(
            Id institutionId,
            Id databaseId
            )
        {
            return
              Result.Ok<AddInstitutionOperatedDatabaseInput, Errors>(
                  new AddInstitutionOperatedDatabaseInput(
                    institutionId: institutionId,
                    databaseId: databaseId
                    )
                  );
        }

        public Id InstitutionId { get => ParentId; }
        public Id DatabaseId { get => AssociateId; }

        private AddInstitutionOperatedDatabaseInput(
            Id institutionId,
            Id databaseId
            )
          : base(
              parentId: institutionId,
              associateId: databaseId
              )
        {
        }
    }
}
