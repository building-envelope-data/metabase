using GreenDonut;
using System.Collections.Generic;
using HotChocolate;
using System.Threading.Tasks;
using ValueObjects = Icon.ValueObjects;

namespace Icon.GraphQl
{
    public abstract class AddOrRemoveComponentVersionPayload
      : Payload
    {
        public ValueObjects.Id BaseComponentId { get; }
        public ValueObjects.Id VersionComponentId { get; }

        public AddOrRemoveComponentVersionPayload(
            ValueObjects.Id baseComponentId,
            ValueObjects.Id versionComponentId,
            ValueObjects.Timestamp requestTimestamp
            )
          : base(requestTimestamp)
        {
            BaseComponentId = baseComponentId;
            VersionComponentId = versionComponentId;
        }

        public Task<Component> GetBaseComponent(
            [DataLoader] ComponentDataLoader componentLoader
            )
        {
            return componentLoader.LoadAsync(
                TimestampHelpers.TimestampId(BaseComponentId, RequestTimestamp)
                );
        }

        public Task<Component> GetVersionComponent(
            [DataLoader] ComponentDataLoader componentLoader
            )
        {
            return componentLoader.LoadAsync(
                TimestampHelpers.TimestampId(VersionComponentId, RequestTimestamp)
                );
        }
    }
}