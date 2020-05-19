using GreenDonut;
using System.Collections.Generic;
using HotChocolate;
using System.Threading.Tasks;
using ValueObjects = Icon.ValueObjects;

namespace Icon.GraphQl
{
    public abstract class AddOrRemoveInstitutionRepresentativePayload
      : Payload
    {
        public ValueObjects.Id InstitutionId { get; }
        public ValueObjects.Id UserId { get; }

        public AddOrRemoveInstitutionRepresentativePayload(
            ValueObjects.Id institutionId,
            ValueObjects.Id userId,
            ValueObjects.Timestamp requestTimestamp
            )
          : base(requestTimestamp)
        {
            InstitutionId = institutionId;
            UserId = userId;
        }

        public Task<Institution> GetInstitution(
            [DataLoader] InstitutionDataLoader institutionLoader
            )
        {
            return institutionLoader.LoadAsync(
                TimestampHelpers.TimestampId(InstitutionId, RequestTimestamp)
                );
        }

        public Task<User> GetUser(/* TODO [DataLoader] UserDataLoader userLoader */)
        {
            return null!;
            /* return userLoader.LoadAsync( */
            /*     TimestampHelpers.TimestampId(UserId, RequestTimestamp) */
            /*     ); */
        }
    }
}