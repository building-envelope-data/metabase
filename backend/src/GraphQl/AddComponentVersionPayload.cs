using GreenDonut;
using System.Collections.Generic;
using HotChocolate;
using System.Threading.Tasks;
using ValueObjects = Icon.ValueObjects;

namespace Icon.GraphQl
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