namespace Icon.GraphQl
{
    public sealed class DeleteCalorimetricDataPayload
      : CreateOrDeleteCalorimetricDataPayload
    {
        public DeleteCalorimetricDataPayload(
            ValueObjects.TimestampedId timestampedId
            )
          : base(timestampedId)
        {
        }
    }
}