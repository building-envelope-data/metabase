namespace Icon.GraphQl
{
    public abstract class CreateOrDeletePhotovoltaicDataPayload
      : Payload
    {
        public ValueObjects.Id PhotovoltaicDataId { get; }

        public CreateOrDeletePhotovoltaicDataPayload(
            ValueObjects.TimestampedId timestampedId
            )
          : base(timestampedId.Timestamp)
        {
            PhotovoltaicDataId = timestampedId.Id;
        }
    }
}