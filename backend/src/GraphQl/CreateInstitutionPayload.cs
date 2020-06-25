using System.Threading.Tasks;
using GreenDonut;
using HotChocolate;

namespace Icon.GraphQl
{
    public sealed class CreateInstitutionPayload
      : CreateOrDeleteInstitutionPayload
    {
        public CreateInstitutionPayload(
            ValueObjects.TimestampedId timestampedId
            )
          : base(timestampedId)
        {
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