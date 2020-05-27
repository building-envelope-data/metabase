using GreenDonut;
using System.Collections.Generic;
using HotChocolate;
using System.Threading.Tasks;
using ValueObjects = Icon.ValueObjects;

namespace Icon.GraphQl
{
    public abstract class AddOrRemovePersonAffiliationPayload
      : Payload
    {
        public ValueObjects.Id PersonId { get; }
        public ValueObjects.Id InstitutionId { get; }

        public AddOrRemovePersonAffiliationPayload(
            ValueObjects.Id personId,
            ValueObjects.Id institutionId,
            ValueObjects.Timestamp requestTimestamp
            )
          : base(requestTimestamp)
        {
            PersonId = personId;
            InstitutionId = institutionId;
        }

        public Task<Person> GetPerson(
            [DataLoader] PersonDataLoader personLoader
            )
        {
            return personLoader.LoadAsync(
              TimestampHelpers.TimestampId(PersonId, RequestTimestamp)
              );
        }

        public Task<Institution> GetInstitution(
            [DataLoader] InstitutionDataLoader institutionLoader
            )
        {
            return institutionLoader.LoadAsync(
              TimestampHelpers.TimestampId(InstitutionId, RequestTimestamp)
              );
        }
    }
}