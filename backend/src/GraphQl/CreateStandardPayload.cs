using GreenDonut;
using System.Collections.Generic;
using HotChocolate;
using System.Threading.Tasks;
using ValueObjects = Icon.ValueObjects;

namespace Icon.GraphQl
{
    public sealed class CreateStandardPayload
      : Payload
    {
        public ValueObjects.Id StandardId { get; }

        public CreateStandardPayload(
            ValueObjects.TimestampedId timestampedId
            )
          : base(timestampedId.Timestamp)
        {
            StandardId = timestampedId.Id;
        }

        public Task<Standard> GetStandard(
            [DataLoader] StandardForTimestampedIdDataLoader standardLoader
            )
        {
            return standardLoader.LoadAsync(
                TimestampHelpers.TimestampId(StandardId, RequestTimestamp)
                );
        }
    }
}