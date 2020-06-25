namespace Icon.GraphQl
{
    public abstract class CreateOrDeleteUserPayload
      : Payload
    {
        public ValueObjects.Id UserId { get; }

        public CreateOrDeleteUserPayload(
            ValueObjects.TimestampedId timestampedId
            )
          : base(timestampedId.Timestamp)
        {
            UserId = timestampedId.Id;
        }
    }
}