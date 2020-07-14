using Infrastructure.GraphQl;
using Infrastructure.ValueObjects;
namespace Metabase.GraphQl
{
    public sealed class RemoveComponentVersionPayload
      : AddOrRemoveComponentVersionPayload
    {
        public RemoveComponentVersionPayload(
            Id baseComponentId,
            Id versionComponentId,
            Timestamp requestTimestamp
            )
          : base(
              baseComponentId: baseComponentId,
              versionComponentId: versionComponentId,
              requestTimestamp: requestTimestamp
              )
        {
        }
    }
}