using System.Threading.Tasks;
using GreenDonut;
using HotChocolate;
using Infrastructure.ValueObjects;

namespace Metabase.GraphQl
{
    public sealed class CreateOpticalDataPayload
      : CreateOrDeleteOpticalDataPayload
    {
        public CreateOpticalDataPayload(
            TimestampedId timestampedId
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