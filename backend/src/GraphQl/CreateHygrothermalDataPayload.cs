using System.Threading.Tasks;
using GreenDonut;
using HotChocolate;

namespace Icon.GraphQl
{
    public sealed class CreateHygrothermalDataPayload
      : CreateOrDeleteHygrothermalDataPayload
    {
        public CreateHygrothermalDataPayload(
            ValueObjects.TimestampedId timestampedId
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