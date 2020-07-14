using System.Threading.Tasks;
using GreenDonut;
using HotChocolate;
using Infrastructure.GraphQl;
using Infrastructure.ValueObjects;

namespace Metabase.GraphQl
{
    public sealed class CreateUserPayload
      : CreateOrDeleteUserPayload
    {
        public CreateUserPayload(
            TimestampedId timestampedId
            )
          : base(timestampedId)
        {
        }

        public Task<User> GetUser(
            [DataLoader] UserDataLoader userLoader
            )
        {
            return userLoader.LoadAsync(
                TimestampHelpers.TimestampId(UserId, RequestTimestamp)
                );
        }
    }
}