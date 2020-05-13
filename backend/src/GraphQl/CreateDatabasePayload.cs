using GreenDonut;
using System.Collections.Generic;
using HotChocolate;
using System.Threading.Tasks;
using ValueObjects = Icon.ValueObjects;

namespace Icon.GraphQl
{
    public sealed class CreateDatabasePayload
      : Payload
    {
        public ValueObjects.Id DatabaseId { get; }

        public CreateDatabasePayload(
            ValueObjects.TimestampedId timestampedId
            )
          : base(timestampedId.Timestamp)
        {
            DatabaseId = timestampedId.Id;
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