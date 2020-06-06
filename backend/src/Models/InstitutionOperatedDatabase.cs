using CSharpFunctionalExtensions;
using DateTime = System.DateTime;
using Errors = Icon.Errors;
using ValueObjects = Icon.ValueObjects;

namespace Icon.Models
{
    public sealed class InstitutionOperatedDatabase
      : Model, IOneToManyAssociation
    {
        public ValueObjects.Id InstitutionId { get; }
        public ValueObjects.Id DatabaseId { get; }

        public ValueObjects.Id ParentId { get => InstitutionId; }
        public ValueObjects.Id AssociateId { get => DatabaseId; }

        private InstitutionOperatedDatabase(
            ValueObjects.Id id,
            ValueObjects.Id institutionId,
            ValueObjects.Id databaseId,
            ValueObjects.Timestamp timestamp
            )
          : base(id, timestamp)
        {
            InstitutionId = institutionId;
            DatabaseId = databaseId;
        }

        public static Result<InstitutionOperatedDatabase, Errors> From(
            ValueObjects.Id id,
            ValueObjects.Id institutionId,
            ValueObjects.Id databaseId,
            ValueObjects.Timestamp timestamp
            )
        {
            return
              Result.Ok<InstitutionOperatedDatabase, Errors>(
                  new InstitutionOperatedDatabase(
            id: id,
            institutionId: institutionId,
            databaseId: databaseId,
            timestamp: timestamp
            )
                  );
        }
    }
}
