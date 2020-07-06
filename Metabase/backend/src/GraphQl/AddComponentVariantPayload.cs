using Infrastructure.ValueObjects;
namespace Metabase.GraphQl
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