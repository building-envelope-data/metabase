using GreenDonut;
using System.Collections.Generic;
using HotChocolate;
using System.Threading.Tasks;
using ValueObjects = Icon.ValueObjects;

namespace Icon.GraphQl
{
    public sealed class RemoveComponentVariantPayload
      : AddOrRemoveComponentVariantPayload
    {
        public RemoveComponentVariantPayload(
            ValueObjects.Id baseComponentId,
            ValueObjects.Id variantComponentId,
            ValueObjects.Timestamp requestTimestamp
            )
          : base(
              baseComponentId: baseComponentId,
              variantComponentId: variantComponentId,
              requestTimestamp: requestTimestamp
              )
        {
        }
    }
}