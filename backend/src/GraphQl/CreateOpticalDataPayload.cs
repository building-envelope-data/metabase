using System.Threading.Tasks;
using GreenDonut;
using HotChocolate;

namespace Icon.GraphQl
{
    public sealed class CreateOpticalDataPayload
      : CreateOrDeleteOpticalDataPayload
    {
        public CreateOpticalDataPayload(
            ValueObjects.TimestampedId timestampedId
            )
          : base(timestampedId)
        {
        }

        public Task<OpticalData> GetOpticalData(
            [DataLoader] OpticalDataDataLoader opticalDataLoader
            )
        {
            return opticalDataLoader.LoadAsync(
                TimestampHelpers.TimestampId(OpticalDataId, RequestTimestamp)
                );
        }
    }
}