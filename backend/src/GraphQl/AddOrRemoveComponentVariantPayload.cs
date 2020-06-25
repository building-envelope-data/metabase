using System.Threading.Tasks;
using GreenDonut;
using HotChocolate;

namespace Icon.GraphQl
{
    public abstract class AddOrRemoveComponentVariantPayload
      : Payload
    {
        public ValueObjects.Id BaseComponentId { get; }
        public ValueObjects.Id VariantComponentId { get; }

        public AddOrRemoveComponentVariantPayload(
            ValueObjects.Id baseComponentId,
            ValueObjects.Id variantComponentId,
            ValueObjects.Timestamp requestTimestamp
            )
          : base(requestTimestamp)
        {
            BaseComponentId = baseComponentId;
            VariantComponentId = variantComponentId;
        }

        public Task<Component> GetBaseComponent(
            [DataLoader] ComponentDataLoader componentLoader
            )
        {
            return componentLoader.LoadAsync(
                TimestampHelpers.TimestampId(BaseComponentId, RequestTimestamp)
                );
        }

        public Task<Component> GetVariantComponent(
            [DataLoader] ComponentDataLoader componentLoader
            )
        {
            return componentLoader.LoadAsync(
                TimestampHelpers.TimestampId(VariantComponentId, RequestTimestamp)
                );
        }
    }
}