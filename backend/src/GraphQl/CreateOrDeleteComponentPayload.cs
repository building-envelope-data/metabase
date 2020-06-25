namespace Icon.GraphQl
{
    public abstract class CreateOrDeleteComponentPayload
      : Payload
    {
        public ValueObjects.Id ComponentId { get; }

        public CreateOrDeleteComponentPayload(
            ValueObjects.TimestampedId timestampedId
            )
          : base(timestampedId.Timestamp)
        {
            ComponentId = timestampedId.Id;
        }
    }
}