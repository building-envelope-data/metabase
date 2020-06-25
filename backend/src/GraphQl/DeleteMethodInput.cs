namespace Icon.GraphQl
{
    public sealed class DeleteMethodInput
      : DeleteNodeInput
    {
        public DeleteMethodInput(
            ValueObjects.Id id,
            ValueObjects.Timestamp timestamp
            )
          : base(id, timestamp)
        {
        }
    }
}