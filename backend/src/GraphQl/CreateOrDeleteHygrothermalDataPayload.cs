namespace Icon.GraphQl
{
    public abstract class CreateOrDeleteHygrothermalDataPayload
      : Payload
    {
        public ValueObjects.Id HygrothermalDataId { get; }

        public CreateOrDeleteHygrothermalDataPayload(
            ValueObjects.TimestampedId timestampedId
            )
          : base(timestampedId.Timestamp)
        {
            HygrothermalDataId = timestampedId.Id;
        }
    }
}