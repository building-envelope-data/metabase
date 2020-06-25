namespace Icon.GraphQl
{
    public sealed class DeletePhotovoltaicDataPayload
      : CreateOrDeletePhotovoltaicDataPayload
    {
        public DeletePhotovoltaicDataPayload(
            ValueObjects.TimestampedId timestampedId
            )
          : base(timestampedId)
        {
        }
    }
}