namespace Icon.GraphQl
{
    public abstract class CreateOrDeleteStandardPayload
      : Payload
    {
        public ValueObjects.Id StandardId { get; }

        public CreateOrDeleteStandardPayload(
            ValueObjects.TimestampedId timestampedId
            )
          : base(timestampedId.Timestamp)
        {
            StandardId = timestampedId.Id;
        }
    }
}