using GreenDonut;
using System.Collections.Generic;
using HotChocolate;
using System.Threading.Tasks;
using ValueObjects = Icon.ValueObjects;

namespace Icon.GraphQl
{
    public sealed class CreateComponentPayload
      : Payload
    {
        public ValueObjects.Id ComponentId { get; }

        public CreateComponentPayload(
            ValueObjects.TimestampedId timestampedId
            )
          : base(timestampedId.Timestamp)
        {
            ComponentId = timestampedId.Id;
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