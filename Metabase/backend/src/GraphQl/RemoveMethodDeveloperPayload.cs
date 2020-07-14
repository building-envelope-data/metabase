using Infrastructure.GraphQl;
using Infrastructure.ValueObjects;
namespace Metabase.GraphQl
{
    public sealed class RemoveMethodDeveloperPayload
      : AddOrRemoveMethodDeveloperPayload
    {
        public RemoveMethodDeveloperPayload(
            Id methodId,
            Id stakeholderId,
            Timestamp requestTimestamp
            )
          : base(
              methodId: methodId,
              stakeholderId: stakeholderId,
              requestTimestamp: requestTimestamp
              )
        {
        }
    }
}