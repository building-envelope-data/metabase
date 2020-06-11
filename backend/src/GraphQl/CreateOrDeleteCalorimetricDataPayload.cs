using System.Collections.Generic;
using System.Threading.Tasks;
using GreenDonut;
using HotChocolate;
using ValueObjects = Icon.ValueObjects;

namespace Icon.GraphQl
{
    public abstract class CreateOrDeleteCalorimetricDataPayload
      : Payload
    {
        public ValueObjects.Id CalorimetricDataId { get; }

        public CreateOrDeleteCalorimetricDataPayload(
            ValueObjects.TimestampedId timestampedId
            )
          : base(timestampedId.Timestamp)
        {
            CalorimetricDataId = timestampedId.Id;
        }
    }
}