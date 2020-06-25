namespace Icon.GraphQl
{
    public abstract class CreateOrDeleteInstitutionPayload
      : Payload
    {
        public ValueObjects.Id InstitutionId { get; }

        public CreateOrDeleteInstitutionPayload(
            ValueObjects.TimestampedId timestampedId
            )
          : base(timestampedId.Timestamp)
        {
            InstitutionId = timestampedId.Id;
        }
    }
}