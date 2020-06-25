namespace Icon.GraphQl
{
    public sealed class DeletePhotovoltaicDataInput
      : DeleteNodeInput
    {
        public DeletePhotovoltaicDataInput(
            ValueObjects.Id id,
            ValueObjects.Timestamp timestamp
            )
          : base(id, timestamp)
        {
        }
    }
}