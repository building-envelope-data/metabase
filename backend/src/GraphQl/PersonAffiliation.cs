using Guid = System.Guid;
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

        public Guid PersonId { get; }
        public Guid InstitutionId { get; }

        public PersonAffiliation(
            Guid id,
            Guid personId,
            Guid institutionId,
            DateTime timestamp,
            DateTime requestTimestamp
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