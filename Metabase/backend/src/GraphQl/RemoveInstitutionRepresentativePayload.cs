using Infrastructure.ValueObjects;
namespace Metabase.GraphQl
{
    public sealed class RemoveInstitutionRepresentativePayload
      : AddOrRemoveInstitutionRepresentativePayload
    {
        public RemoveInstitutionRepresentativePayload(
            Id institutionId,
            Id userId,
            Timestamp requestTimestamp
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