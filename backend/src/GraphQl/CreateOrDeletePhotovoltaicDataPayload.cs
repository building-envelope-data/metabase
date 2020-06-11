using System.Collections.Generic;
using System.Threading.Tasks;
using GreenDonut;
using HotChocolate;
using ValueObjects = Icon.ValueObjects;

namespace Icon.GraphQl
{
    public abstract class CreateOrDeletePhotovoltaicDataPayload
      : Payload
    {
        public ValueObjects.Id PhotovoltaicDataId { get; }

        public CreateOrDeletePhotovoltaicDataPayload(
            ValueObjects.TimestampedId timestampedId
            )
          : base(timestampedId.Timestamp)
        {
            PhotovoltaicDataId = timestampedId.Id;
        }
    }
}