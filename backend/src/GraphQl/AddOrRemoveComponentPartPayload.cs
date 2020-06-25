using System.Threading.Tasks;
using GreenDonut;
using HotChocolate;

namespace Icon.GraphQl
{
    public abstract class AddOrRemoveComponentPartPayload
      : Payload
    {
        public ValueObjects.Id AssembledComponentId { get; }
        public ValueObjects.Id PartComponentId { get; }

        public AddOrRemoveComponentPartPayload(
            ValueObjects.Id assembledComponentId,
            ValueObjects.Id partComponentId,
            ValueObjects.Timestamp requestTimestamp
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