using Infrastructure.ValueObjects;
namespace Metabase.GraphQl
{
    public sealed class AddComponentVersionPayload
      : AddOrRemoveComponentVersionPayload
    {
        public AddComponentVersionPayload(
            ComponentVersion componentVersion
            )
          : base(
              baseComponentId: componentVersion.BaseComponentId,
              versionComponentId: componentVersion.VersionComponentId,
              requestTimestamp: componentVersion.RequestTimestamp
              )
        {
        }
    }
}