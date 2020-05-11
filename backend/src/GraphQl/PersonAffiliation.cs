using Models = Icon.Models;
using DateTime = System.DateTime;

namespace Icon.GraphQl
{
    public sealed class PersonAffiliation
      : NodeBase
    {
        public static PersonAffiliation FromModel(
            Models.PersonAffiliation model,
            ValueObjects.Timestamp requestTimestamp
            )
        {
            return new PersonAffiliation(
                id: model.Id,
                personId: model.PersonId,
                institutionId: model.InstitutionId,
                timestamp: model.Timestamp,
                requestTimestamp: requestTimestamp
                );
        }

        public ValueObjects.Id PersonId { get; }
        public ValueObjects.Id InstitutionId { get; }

        public PersonAffiliation(
            ValueObjects.Id id,
            ValueObjects.Id personId,
            ValueObjects.Id institutionId,
            ValueObjects.Timestamp timestamp,
            ValueObjects.Timestamp requestTimestamp
            )
          : base(
              id: id,
              timestamp: timestamp,
              requestTimestamp: requestTimestamp
              )
        {
            PersonId = personId;
            InstitutionId = institutionId;
        }
    }
}