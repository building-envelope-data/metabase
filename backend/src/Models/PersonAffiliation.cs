using ValueObjects = Icon.ValueObjects;
using Errors = Icon.Errors;
using CSharpFunctionalExtensions;
using DateTime = System.DateTime;

namespace Icon.Models
{
    public sealed class PersonAffiliation
      : Model
    {
        public ValueObjects.Id PersonId { get; }
        public ValueObjects.Id InstitutionId { get; }

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