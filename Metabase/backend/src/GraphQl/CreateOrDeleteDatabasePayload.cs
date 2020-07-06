using Infrastructure.ValueObjects;
namespace Metabase.GraphQl
{
    public abstract class CreateOrDeleteDatabasePayload
      : Payload
    {
        public Id DatabaseId { get; }

        public CreateOrDeleteDatabasePayload(
            TimestampedId timestampedId
            )
          : base(timestampedId.Timestamp)
        {
            DatabaseId = timestampedId.Id;
        }
    }
}