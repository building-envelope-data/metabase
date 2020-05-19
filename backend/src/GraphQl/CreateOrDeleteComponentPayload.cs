using GreenDonut;
using System.Collections.Generic;
using HotChocolate;
using System.Threading.Tasks;
using ValueObjects = Icon.ValueObjects;

namespace Icon.GraphQl
{
    public abstract class CreateOrDeleteComponentPayload
      : Payload
    {
        public ValueObjects.Id ComponentId { get; }

        public CreateOrDeleteComponentPayload(
            ValueObjects.TimestampedId timestampedId
            )
          : base(timestampedId.Timestamp)
        {
            ComponentId = timestampedId.Id;
        }
    }
}