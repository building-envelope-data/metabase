namespace Icon.GraphQl
{
    public sealed class DeleteOpticalDataPayload
      : CreateOrDeleteOpticalDataPayload
    {
        public DeleteOpticalDataPayload(
            ValueObjects.TimestampedId timestampedId
            )
          : base(timestampedId)
        {
        }
    }
}