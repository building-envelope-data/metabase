using System.Threading.Tasks;
using GreenDonut;
using HotChocolate;
using Infrastructure.ValueObjects;

namespace Metabase.GraphQl
{
    public abstract class AddOrRemoveComponentVersionPayload
      : Payload
    {
        public Id BaseComponentId { get; }
        public Id VersionComponentId { get; }

        protected AddOrRemoveComponentVersionPayload(
            Id baseComponentId,
            Id versionComponentId,
            Timestamp requestTimestamp
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