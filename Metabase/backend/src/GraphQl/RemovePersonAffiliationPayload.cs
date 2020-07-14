using Infrastructure.GraphQl;
using Infrastructure.ValueObjects;
namespace Metabase.GraphQl
{
    public sealed class RemovePersonAffiliationPayload
      : AddOrRemovePersonAffiliationPayload
    {
        public RemovePersonAffiliationPayload(
            Id personId,
            Id institutionId,
            Timestamp requestTimestamp
            )
          : base(
              personId: personId,
              institutionId: institutionId,
              requestTimestamp: requestTimestamp
              )
        {
        }
    }
}