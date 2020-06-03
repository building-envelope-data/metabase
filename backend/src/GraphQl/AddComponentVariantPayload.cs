using System.Collections.Generic;
using System.Threading.Tasks;
using GreenDonut;
using HotChocolate;
using ValueObjects = Icon.ValueObjects;

namespace Icon.GraphQl
{
    public sealed class AddComponentVariantPayload
      : AddOrRemoveComponentVariantPayload
    {
        public AddComponentVariantPayload(
            ComponentVariant componentVariant
            )
          : base(
              baseComponentId: componentVariant.BaseComponentId,
              variantComponentId: componentVariant.VariantComponentId,
              requestTimestamp: componentVariant.RequestTimestamp
              )
        {
        }
    }
}