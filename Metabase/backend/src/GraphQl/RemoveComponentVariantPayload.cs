using Infrastructure.GraphQl;
using Infrastructure.ValueObjects;
namespace Metabase.GraphQl
{
    public sealed class RemoveComponentVariantPayload
      : AddOrRemoveComponentVariantPayload
    {
        public RemoveComponentVariantPayload(
            Id baseComponentId,
            Id variantComponentId,
            Timestamp requestTimestamp
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