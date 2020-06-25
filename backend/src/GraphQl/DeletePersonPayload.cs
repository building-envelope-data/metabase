namespace Icon.GraphQl
{
    public sealed class DeletePersonPayload
      : CreateOrDeletePersonPayload
    {
        public DeletePersonPayload(
            ValueObjects.TimestampedId timestampedId
            )
          : base(timestampedId)
        {
        }
    }
}