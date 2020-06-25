namespace Icon.GraphQl
{
    public sealed class DeleteComponentInput
      : DeleteNodeInput
    {
        public DeleteComponentInput(
            ValueObjects.Id id,
            ValueObjects.Timestamp timestamp
            )
          : base(id, timestamp)
        {
        }
    }
}