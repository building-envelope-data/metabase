namespace Icon.GraphQl
{
    public sealed class RemoveInstitutionRepresentativePayload
      : AddOrRemoveInstitutionRepresentativePayload
    {
        public RemoveInstitutionRepresentativePayload(
            ValueObjects.Id institutionId,
            ValueObjects.Id userId,
            ValueObjects.Timestamp requestTimestamp
            )
          : base(
              institutionId: institutionId,
              userId: userId,
              requestTimestamp: requestTimestamp
              )
        {
        }
    }
}