using System.Threading.Tasks;
using GreenDonut;
using HotChocolate;

namespace Icon.GraphQl
{
    public sealed class CreateDatabasePayload
      : CreateOrDeleteDatabasePayload
    {
        public CreateDatabasePayload(
            ValueObjects.TimestampedId timestampedId
            )
          : base(timestampedId)
        {
        }

        public Task<Database> GetDatabase(
            [DataLoader] DatabaseDataLoader databaseLoader
            )
        {
            return databaseLoader.LoadAsync(
                TimestampHelpers.TimestampId(DatabaseId, RequestTimestamp)
                );
        }
    }
}