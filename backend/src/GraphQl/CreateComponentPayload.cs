using GreenDonut;
using System.Collections.Generic;
using HotChocolate;
using System.Threading.Tasks;
using ValueObjects = Icon.ValueObjects;

namespace Icon.GraphQl
{
    public sealed class CreateComponentPayload
      : CreateOrDeleteComponentPayload
    {
        public CreateComponentPayload(
            ValueObjects.TimestampedId timestampedId
            )
          : base(timestampedId)
        {
        }

        public Task<Component> GetComponent(
            [DataLoader] ComponentDataLoader componentLoader
            )
        {
            return componentLoader.LoadAsync(
                TimestampHelpers.TimestampId(ComponentId, RequestTimestamp)
                );
        }
    }
}