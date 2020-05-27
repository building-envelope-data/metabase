using GreenDonut;
using System.Collections.Generic;
using HotChocolate;
using System.Threading.Tasks;
using ValueObjects = Icon.ValueObjects;

namespace Icon.GraphQl
{
    public abstract class CreateOrDeleteMethodPayload
      : Payload
    {
        public ValueObjects.Id MethodId { get; }

        public CreateOrDeleteMethodPayload(
            ValueObjects.TimestampedId timestampedId
            )
          : base(timestampedId.Timestamp)
        {
            MethodId = timestampedId.Id;
        }
    }
}