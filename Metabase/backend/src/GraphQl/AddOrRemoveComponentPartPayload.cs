using System.Threading.Tasks;
using GreenDonut;
using HotChocolate;
using Infrastructure.ValueObjects;

namespace Metabase.GraphQl
{
    public abstract class AddOrRemoveComponentPartPayload
      : Payload
    {
        public Id AssembledComponentId { get; }
        public Id PartComponentId { get; }

        public AddOrRemoveComponentPartPayload(
            Id assembledComponentId,
            Id partComponentId,
            Timestamp requestTimestamp
            )
          : base(requestTimestamp)
        {
            AssembledComponentId = assembledComponentId;
            PartComponentId = partComponentId;
        }

        public Task<Component> GetAssembledComponent(
            [DataLoader] ComponentDataLoader componentLoader
            )
        {
            return componentLoader.LoadAsync(
                TimestampHelpers.TimestampId(AssembledComponentId, RequestTimestamp)
                );
        }

        public Task<Component> GetPartComponent(
            [DataLoader] ComponentDataLoader componentLoader
            )
        {
            return componentLoader.LoadAsync(
                TimestampHelpers.TimestampId(PartComponentId, RequestTimestamp)
                );
        }
    }
}