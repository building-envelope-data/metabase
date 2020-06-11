using System.Collections.Generic;
using System.Threading.Tasks;
using GreenDonut;
using HotChocolate;
using ValueObjects = Icon.ValueObjects;

namespace Icon.GraphQl
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