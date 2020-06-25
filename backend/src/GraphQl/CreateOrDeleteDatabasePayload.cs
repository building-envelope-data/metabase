namespace Icon.GraphQl
{
    public abstract class CreateOrDeleteDatabasePayload
      : Payload
    {
        public ValueObjects.Id DatabaseId { get; }

        public CreateOrDeleteDatabasePayload(
            ValueObjects.TimestampedId timestampedId
            )
          : base(timestampedId.Timestamp)
        {
            DatabaseId = timestampedId.Id;
        }
    }
}