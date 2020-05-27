using GreenDonut;
using System.Collections.Generic;
using HotChocolate;
using System.Threading.Tasks;
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