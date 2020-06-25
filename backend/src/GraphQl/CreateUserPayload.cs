using System.Threading.Tasks;
using GreenDonut;
using HotChocolate;

namespace Icon.GraphQl
{
    public sealed class CreateUserPayload
      : CreateOrDeleteUserPayload
    {
        public CreateUserPayload(
            ValueObjects.TimestampedId timestampedId
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