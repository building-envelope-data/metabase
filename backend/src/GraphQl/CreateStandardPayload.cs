using System.Collections.Generic;
using System.Threading.Tasks;
using GreenDonut;
using HotChocolate;
using ValueObjects = Icon.ValueObjects;

namespace Icon.GraphQl
{
    public sealed class CreateStandardPayload
      : CreateOrDeleteStandardPayload
    {
        public CreateStandardPayload(
            ValueObjects.TimestampedId timestampedId
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