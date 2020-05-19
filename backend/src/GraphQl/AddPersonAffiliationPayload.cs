using GreenDonut;
using System.Collections.Generic;
using HotChocolate;
using System.Threading.Tasks;
using ValueObjects = Icon.ValueObjects;

namespace Icon.GraphQl
{
    public sealed class AddPersonAffiliationPayload
      : AddOrRemovePersonAffiliationPayload
    {
        public AddPersonAffiliationPayload(
            PersonAffiliation personAffiliation
            )
          : base(
              personId: personAffiliation.PersonId,
              institutionId: personAffiliation.InstitutionId,
              requestTimestamp: personAffiliation.RequestTimestamp
              )
        {
        }
    }
}