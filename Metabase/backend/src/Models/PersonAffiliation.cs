using CSharpFunctionalExtensions;
using Infrastructure.Models;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Metabase.Models
{
    public sealed class PersonAffiliation
      : Model, IManyToManyAssociation
    {
        public Id PersonId { get; }
        public Id InstitutionId { get; }

        public Id ParentId { get => PersonId; }
        public Id AssociateId { get => InstitutionId; }

        private PersonAffiliation(
            Id id,
            Id personId,
            Id institutionId,
            Timestamp timestamp
            )
          : base(id, timestamp)
        {
            PersonId = personId;
            InstitutionId = institutionId;
        }

        public static Result<PersonAffiliation, Errors> From(
            Id id,
            Id personId,
            Id institutionId,
            Timestamp timestamp
            )
        {
            return
              Result.Ok<PersonAffiliation, Errors>(
                  new PersonAffiliation(
            id: id,
            personId: personId,
            institutionId: institutionId,
            timestamp: timestamp
            )
                  );
        }
    }
}