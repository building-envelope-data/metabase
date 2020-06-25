namespace Icon.GraphQl
{
    public abstract class CreateOrDeletePersonPayload
      : Payload
    {
        public ValueObjects.Id PersonId { get; }

        public CreateOrDeletePersonPayload(
            ValueObjects.TimestampedId timestampedId
            )
          : base(timestampedId.Timestamp)
        {
            PersonId = timestampedId.Id;
        }
    }
}