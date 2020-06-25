namespace Icon.GraphQl
{
    public sealed class DeleteMethodPayload
      : CreateOrDeleteMethodPayload
    {
        public DeleteMethodPayload(
            ValueObjects.TimestampedId timestampedId
            )
          : base(timestampedId)
        {
        }
    }
}