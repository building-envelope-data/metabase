using System.Threading.Tasks;
using GreenDonut;
using HotChocolate;
using Infrastructure.ValueObjects;

namespace Metabase.GraphQl
{
    public sealed class CreateHygrothermalDataPayload
      : CreateOrDeleteHygrothermalDataPayload
    {
        public CreateHygrothermalDataPayload(
            TimestampedId timestampedId
            )
          : base(timestampedId)
        {
        }

        public Task<HygrothermalData> GetHygrothermalData(
            [DataLoader] HygrothermalDataDataLoader hygrothermalDataLoader
            )
        {
            return hygrothermalDataLoader.LoadAsync(
                TimestampHelpers.TimestampId(HygrothermalDataId, RequestTimestamp)
                );
        }
    }
}