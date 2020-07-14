using System.Threading.Tasks;
using GreenDonut;
using HotChocolate;
using Infrastructure.GraphQl;
using Infrastructure.ValueObjects;

namespace Metabase.GraphQl
{
    public abstract class AddOrRemoveComponentVariantPayload
      : Payload
    {
        public Id BaseComponentId { get; }
        public Id VariantComponentId { get; }

        protected AddOrRemoveComponentVariantPayload(
            Id baseComponentId,
            Id variantComponentId,
            Timestamp requestTimestamp
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