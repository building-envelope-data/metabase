using GreenDonut;
using System.Collections.Generic;
using HotChocolate;
using System.Threading.Tasks;
using ValueObjects = Icon.ValueObjects;

namespace Icon.GraphQl
{
    public abstract class CreateOrDeletePersonPayload
      : Payload
    {
        public ValueObjects.Id PersonId { get; }

        public CreateOrDeletePersonPayload(
            ValueObjects.TimestampedId timestampedId
            )
          : base(timestampedId.Timestamp)
        {
            PersonId = timestampedId.Id;
        }
    }
}