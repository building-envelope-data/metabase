namespace Icon.GraphQl
{
    public sealed class DeleteUserPayload
      : CreateOrDeleteUserPayload
    {
        public DeleteUserPayload(
            ValueObjects.TimestampedId timestampedId
            )
          : base(timestampedId)
        {
        }
    }
}