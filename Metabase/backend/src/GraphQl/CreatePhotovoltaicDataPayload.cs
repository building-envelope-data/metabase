using System.Threading.Tasks;
using GreenDonut;
using HotChocolate;
using Infrastructure.ValueObjects;

namespace Metabase.GraphQl
{
    public sealed class CreatePhotovoltaicDataPayload
      : CreateOrDeletePhotovoltaicDataPayload
    {
        public CreatePhotovoltaicDataPayload(
            TimestampedId timestampedId
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