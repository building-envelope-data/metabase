namespace Icon.GraphQl
{
    public sealed class DeleteInstitutionPayload
      : CreateOrDeleteInstitutionPayload
    {
        public DeleteInstitutionPayload(
            ValueObjects.TimestampedId timestampedId
            )
          : base(timestampedId)
        {
        }
    }
}