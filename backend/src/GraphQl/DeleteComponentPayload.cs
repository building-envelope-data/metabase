namespace Icon.GraphQl
{
    public sealed class DeleteComponentPayload
      : CreateOrDeleteComponentPayload
    {
        public DeleteComponentPayload(
            ValueObjects.TimestampedId timestampedId
            )
          : base(timestampedId)
        {
        }
    }
}