using GreenDonut;
using System.Collections.Generic;
using HotChocolate;
using System.Threading.Tasks;
using ValueObjects = Icon.ValueObjects;

namespace Icon.GraphQl
{
    public sealed class AddPersonAffiliationPayload
      : Payload
    {
        public ValueObjects.Id PersonId { get; }
        public ValueObjects.Id InstitutionId { get; }

        public AddPersonAffiliationPayload(
            PersonAffiliation personAffiliation
            )
          : base(personAffiliation.RequestTimestamp)
        {
            PersonId = personAffiliation.PersonId;
            InstitutionId = personAffiliation.InstitutionId;
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