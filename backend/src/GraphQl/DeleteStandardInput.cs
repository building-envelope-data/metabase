namespace Icon.GraphQl
{
    public sealed class DeleteStandardInput
      : DeleteNodeInput
    {
        public DeleteStandardInput(
            ValueObjects.Id id,
            ValueObjects.Timestamp timestamp
            )
          : base(id, timestamp)
        {
        }
    }
}