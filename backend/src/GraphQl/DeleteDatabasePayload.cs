namespace Icon.GraphQl
{
    public sealed class DeleteDatabasePayload
      : CreateOrDeleteDatabasePayload
    {
        public DeleteDatabasePayload(
            ValueObjects.TimestampedId timestampedId
            )
          : base(timestampedId)
        {
        }
    }
}