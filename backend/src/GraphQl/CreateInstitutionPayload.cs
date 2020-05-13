using GreenDonut;
using System.Collections.Generic;
using HotChocolate;
using System.Threading.Tasks;
using ValueObjects = Icon.ValueObjects;

namespace Icon.GraphQl
{
    public sealed class CreateInstitutionPayload
      : Payload
    {
        public ValueObjects.Id InstitutionId { get; }

        public CreateInstitutionPayload(
            ValueObjects.TimestampedId timestampedId
            )
          : base(timestampedId.Timestamp)
        {
            InstitutionId = timestampedId.Id;
        }

        public Task<Institution> GetInstitution(
            [DataLoader] InstitutionDataLoader institutionLoader
            )
        {
            return institutionLoader.LoadAsync(
                TimestampHelpers.TimestampId(InstitutionId, RequestTimestamp)
                );
        }
    }
}