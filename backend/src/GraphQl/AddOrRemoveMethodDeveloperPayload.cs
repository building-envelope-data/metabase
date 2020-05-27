using GreenDonut;
using System.Collections.Generic;
using HotChocolate;
using System.Threading.Tasks;
using ValueObjects = Icon.ValueObjects;

namespace Icon.GraphQl
{
    public abstract class AddOrRemoveMethodDeveloperPayload
      : Payload
    {
        public ValueObjects.Id MethodId { get; }
        public ValueObjects.Id StakeholderId { get; }

        public AddOrRemoveMethodDeveloperPayload(
            ValueObjects.Id methodId,
            ValueObjects.Id stakeholderId,
            ValueObjects.Timestamp requestTimestamp
            )
          : base(requestTimestamp)
        {
            MethodId = methodId;
            StakeholderId = stakeholderId;
        }

        public Task<Method> GetMethod(
            [DataLoader] MethodDataLoader methodLoader
            )
        {
            return methodLoader.LoadAsync(
                TimestampHelpers.TimestampId(MethodId, RequestTimestamp)
                );
        }

        public Task<Stakeholder> GetStakeholder(
            [DataLoader] StakeholderDataLoader stakeholderLoader
            )
        {
            return stakeholderLoader.LoadAsync(
                TimestampHelpers.TimestampId(StakeholderId, RequestTimestamp)
                );
        }
    }
}