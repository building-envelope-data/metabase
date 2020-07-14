using CSharpFunctionalExtensions;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Metabase.ValueObjects
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