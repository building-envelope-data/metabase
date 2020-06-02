using GreenDonut;
using System.Collections.Generic;
using HotChocolate;
using System.Threading.Tasks;
using ValueObjects = Icon.ValueObjects;

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
