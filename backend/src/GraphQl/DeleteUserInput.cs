namespace Icon.GraphQl
{
    public sealed class DeleteUserInput
      : DeleteNodeInput
    {
        public DeleteUserInput(
            ValueObjects.Id id,
            ValueObjects.Timestamp timestamp
            )
          : base(id, timestamp)
        {
        }
    }
}