using System.Threading.Tasks;
using GreenDonut;
using HotChocolate;
using Infrastructure.ValueObjects;

namespace Metabase.GraphQl
{
    public abstract class AddOrRemovePersonAffiliationPayload
      : Payload
    {
        public Id PersonId { get; }
        public Id InstitutionId { get; }

        protected AddOrRemovePersonAffiliationPayload(
            Id personId,
            Id institutionId,
            Timestamp requestTimestamp
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