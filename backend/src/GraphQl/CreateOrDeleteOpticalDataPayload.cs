namespace Icon.GraphQl
{
    public abstract class CreateOrDeleteOpticalDataPayload
      : Payload
    {
        public ValueObjects.Id OpticalDataId { get; }

        public CreateOrDeleteOpticalDataPayload(
            ValueObjects.TimestampedId timestampedId
            )
          : base(timestampedId.Timestamp)
        {
            OpticalDataId = timestampedId.Id;
        }
    }
}