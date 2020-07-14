using System.Threading.Tasks;
using GreenDonut;
using HotChocolate;
using Infrastructure.GraphQl;
using Infrastructure.ValueObjects;

namespace Metabase.GraphQl
{
    public sealed class CreateDatabasePayload
      : CreateOrDeleteDatabasePayload
    {
        public CreateDatabasePayload(
            TimestampedId timestampedId
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