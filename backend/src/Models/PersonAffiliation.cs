using CSharpFunctionalExtensions;
using Icon.Infrastructure.Models;

namespace Icon.Models
{
    public sealed class PersonAffiliation
      : Model, IManyToManyAssociation
    {
        public ValueObjects.Id PersonId { get; }
        public ValueObjects.Id InstitutionId { get; }

        public ValueObjects.Id ParentId { get => PersonId; }
        public ValueObjects.Id AssociateId { get => InstitutionId; }

        private PersonAffiliation(
            ValueObjects.Id id,
            ValueObjects.Id personId,
            ValueObjects.Id institutionId,
            ValueObjects.Timestamp timestamp
            )
          : base(id, timestamp)
        {
            PersonId = personId;
            InstitutionId = institutionId;
        }

        public static Result<PersonAffiliation, Errors> From(
            ValueObjects.Id id,
            ValueObjects.Id personId,
            ValueObjects.Id institutionId,
            ValueObjects.Timestamp timestamp
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