namespace Icon.GraphQl
{
    public sealed class DeleteDatabaseInput
      : DeleteNodeInput
    {
        public DeleteDatabaseInput(
            ValueObjects.Id id,
            ValueObjects.Timestamp timestamp
            )
          : base(id, timestamp)
        {
        }
    }
}