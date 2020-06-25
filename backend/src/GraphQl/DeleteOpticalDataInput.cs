namespace Icon.GraphQl
{
    public sealed class DeleteOpticalDataInput
      : DeleteNodeInput
    {
        public DeleteOpticalDataInput(
            ValueObjects.Id id,
            ValueObjects.Timestamp timestamp
            )
          : base(id, timestamp)
        {
        }
    }
}