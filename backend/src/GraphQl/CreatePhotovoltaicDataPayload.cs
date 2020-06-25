using System.Threading.Tasks;
using GreenDonut;
using HotChocolate;

namespace Icon.GraphQl
{
    public sealed class CreatePhotovoltaicDataPayload
      : CreateOrDeletePhotovoltaicDataPayload
    {
        public CreatePhotovoltaicDataPayload(
            ValueObjects.TimestampedId timestampedId
            )
          : base(timestampedId)
        {
        }

        public Task<PhotovoltaicData> GetPhotovoltaicData(
            [DataLoader] PhotovoltaicDataDataLoader photovoltaicDataLoader
            )
        {
            return photovoltaicDataLoader.LoadAsync(
                TimestampHelpers.TimestampId(PhotovoltaicDataId, RequestTimestamp)
                );
        }
    }
}