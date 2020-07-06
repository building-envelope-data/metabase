using System.Threading.Tasks;
using GreenDonut;
using HotChocolate;
using Infrastructure.ValueObjects;

namespace Metabase.GraphQl
{
    public sealed class CreateCalorimetricDataPayload
      : CreateOrDeleteCalorimetricDataPayload
    {
        public CreateCalorimetricDataPayload(
            TimestampedId timestampedId
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