using GreenDonut;
using System.Collections.Generic;
using HotChocolate;
using System.Threading.Tasks;
using ValueObjects = Icon.ValueObjects;

namespace Icon.GraphQl
{
    public abstract class CreateOrDeleteUserPayload
      : Payload
    {
        public ValueObjects.Id UserId { get; }

        public CreateOrDeleteUserPayload(
            ValueObjects.TimestampedId timestampedId
            )
          : base(timestampedId.Timestamp)
        {
            UserId = timestampedId.Id;
        }
    }
}