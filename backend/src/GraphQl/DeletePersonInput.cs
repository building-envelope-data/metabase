namespace Icon.GraphQl
{
    public sealed class DeletePersonInput
      : DeleteNodeInput
    {
        public DeletePersonInput(
            ValueObjects.Id id,
            ValueObjects.Timestamp timestamp
            )
          : base(id, timestamp)
        {
        }
    }
}