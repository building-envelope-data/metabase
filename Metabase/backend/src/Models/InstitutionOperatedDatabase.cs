using CSharpFunctionalExtensions;
using Infrastructure.Models;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Metabase.Models
{
    public sealed class InstitutionOperatedDatabase
      : Model, IOneToManyAssociation
    {
        public Id InstitutionId { get; }
        public Id DatabaseId { get; }

        public Id ParentId { get => InstitutionId; }
        public Id AssociateId { get => DatabaseId; }

        private InstitutionOperatedDatabase(
            Id id,
            Id institutionId,
            Id databaseId,
            Timestamp timestamp
            )
          : base(id, timestamp)
        {
            InstitutionId = institutionId;
            DatabaseId = databaseId;
        }

        public static Result<InstitutionOperatedDatabase, Errors> From(
            Id id,
            Id institutionId,
            Id databaseId,
            Timestamp timestamp
            )
        {
            return
              Result.Success<InstitutionOperatedDatabase, Errors>(
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