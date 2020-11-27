using Infrastructure.GraphQl;
using Infrastructure.ValueObjects;
namespace Metabase.GraphQl
{
    public sealed class AddMethodDeveloperPayload
      : AddOrRemoveMethodDeveloperPayload
    {
        public AddMethodDeveloperPayload(
            MethodDeveloper methodDeveloper
            )
          : base(
              methodId: methodDeveloper.MethodId,
              stakeholderId: methodDeveloper.StakeholderId,
              requestTimestamp: methodDeveloper.RequestTimestamp
              )
        {
        }
    }
}