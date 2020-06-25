namespace Icon.GraphQl
{
    public sealed class DeleteHygrothermalDataPayload
      : CreateOrDeleteHygrothermalDataPayload
    {
        public DeleteHygrothermalDataPayload(
            ValueObjects.TimestampedId timestampedId
            )
          : base(timestampedId)
        {
        }
    }
}