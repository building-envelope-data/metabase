using System.Threading.Tasks;
using GreenDonut;
using HotChocolate;
using Infrastructure.ValueObjects;

namespace Metabase.GraphQl
{
    public sealed class CreateStandardPayload
      : CreateOrDeleteStandardPayload
    {
        public CreateStandardPayload(
            TimestampedId timestampedId
            )
          : base(timestampedId)
        {
        }

        public Task<Standard> GetStandard(
            [DataLoader] StandardDataLoader standardLoader
            )
        {
            return standardLoader.LoadAsync(
                TimestampHelpers.TimestampId(StandardId, RequestTimestamp)
                );
        }
    }
}