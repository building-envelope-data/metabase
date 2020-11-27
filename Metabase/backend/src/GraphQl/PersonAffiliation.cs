using Infrastructure.GraphQl;
using Infrastructure.ValueObjects;
namespace Metabase.GraphQl
{
    public sealed class PersonAffiliation
      : Node
    {
        public static PersonAffiliation FromModel(
            Models.PersonAffiliation model,
            Timestamp requestTimestamp
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

        public Id PersonId { get; }
        public Id InstitutionId { get; }

        public PersonAffiliation(
            Id id,
            Id personId,
            Id institutionId,
            Timestamp timestamp,
            Timestamp requestTimestamp
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