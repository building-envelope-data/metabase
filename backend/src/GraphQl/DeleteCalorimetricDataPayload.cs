using System.Collections.Generic;
using System.Threading.Tasks;
using GreenDonut;
using HotChocolate;
using ValueObjects = Icon.ValueObjects;

namespace Icon.GraphQl
{
    public sealed class DeleteCalorimetricDataPayload
      : CreateOrDeleteCalorimetricDataPayload
    {
        public DeleteCalorimetricDataPayload(
            ValueObjects.TimestampedId timestampedId
            )
          : base(timestampedId)
        {
        }
    }
}