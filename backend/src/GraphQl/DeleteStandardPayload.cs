namespace Icon.GraphQl
{
    public sealed class DeleteStandardPayload
      : CreateOrDeleteStandardPayload
    {
        public DeleteStandardPayload(
            ValueObjects.TimestampedId timestampedId
            )
          : base(timestampedId)
        {
        }
    }
}