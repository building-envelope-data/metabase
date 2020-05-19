using GreenDonut;
using System.Collections.Generic;
using HotChocolate;
using System.Threading.Tasks;
using ValueObjects = Icon.ValueObjects;

namespace Icon.GraphQl
{
    public sealed class RemoveComponentManufacturerPayload
      : AddOrRemoveComponentManufacturerPayload
    {
        public RemoveComponentManufacturerPayload(
            ValueObjects.Id componentId,
            ValueObjects.Id institutionId,
            ValueObjects.Timestamp requestTimestamp
            )
          : base(
              componentId: componentId,
              institutionId: institutionId,
              requestTimestamp: requestTimestamp
              )
        {
        }
    }
}