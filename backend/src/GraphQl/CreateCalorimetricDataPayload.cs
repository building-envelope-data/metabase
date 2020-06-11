using System.Collections.Generic;
using System.Threading.Tasks;
using GreenDonut;
using HotChocolate;
using ValueObjects = Icon.ValueObjects;

namespace Icon.GraphQl
{
    public sealed class CreateCalorimetricDataPayload
      : CreateOrDeleteCalorimetricDataPayload
    {
        public CreateCalorimetricDataPayload(
            ValueObjects.TimestampedId timestampedId
            )
          : base(timestampedId)
        {
        }

        public Task<CalorimetricData> GetCalorimetricData(
            [DataLoader] CalorimetricDataDataLoader calorimetricDataLoader
            )
        {
            return calorimetricDataLoader.LoadAsync(
                TimestampHelpers.TimestampId(CalorimetricDataId, RequestTimestamp)
                );
        }
    }
}