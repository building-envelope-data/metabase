using System.Collections.Generic;
using System.Threading.Tasks;
using GreenDonut;
using HotChocolate;
using ValueObjects = Icon.ValueObjects;

namespace Icon.GraphQl
{
    public sealed class RemovePersonAffiliationPayload
      : AddOrRemovePersonAffiliationPayload
    {
        public RemovePersonAffiliationPayload(
            ValueObjects.Id personId,
            ValueObjects.Id institutionId,
            ValueObjects.Timestamp requestTimestamp
            )
          : base(
              personId: personId,
              institutionId: institutionId,
              requestTimestamp: requestTimestamp
              )
        {
        }
    }
}