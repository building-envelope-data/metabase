namespace Icon.GraphQl
{
    public sealed class DeleteCalorimetricDataInput
      : DeleteNodeInput
    {
        public DeleteCalorimetricDataInput(
            ValueObjects.Id id,
            ValueObjects.Timestamp timestamp
            )
          : base(id, timestamp)
        {
        }
    }
}