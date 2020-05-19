using GreenDonut;
using System.Collections.Generic;
using HotChocolate;
using System.Threading.Tasks;
using ValueObjects = Icon.ValueObjects;

namespace Icon.GraphQl
{
    public sealed class RemoveMethodDeveloperPayload
      : AddOrRemoveMethodDeveloperPayload
    {
        public RemoveMethodDeveloperPayload(
            ValueObjects.Id methodId,
            ValueObjects.Id stakeholderId,
            ValueObjects.Timestamp requestTimestamp
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