using System.Threading.Tasks;
using GreenDonut;
using HotChocolate;
using Infrastructure.GraphQl;
using Infrastructure.ValueObjects;

namespace Metabase.GraphQl
{
    public abstract class AddOrRemoveInstitutionRepresentativePayload
      : Payload
    {
        public Id InstitutionId { get; }
        public Id UserId { get; }

        protected AddOrRemoveInstitutionRepresentativePayload(
            Id institutionId,
            Id userId,
            Timestamp requestTimestamp
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