namespace Icon.GraphQl
{
    public sealed class DeleteHygrothermalDataInput
      : DeleteNodeInput
    {
        public DeleteHygrothermalDataInput(
            ValueObjects.Id id,
            ValueObjects.Timestamp timestamp
            )
          : base(id, timestamp)
        {
        }
    }
}