using GreenDonut;
using System.Collections.Generic;
using HotChocolate;
using System.Threading.Tasks;
using ValueObjects = Icon.ValueObjects;

namespace Icon.GraphQl
{
    public sealed class AddMethodDeveloperPayload
      : Payload
    {
        public ValueObjects.Id MethodId { get; }
        public ValueObjects.Id StakeholderId { get; }

        public AddMethodDeveloperPayload(
            ValueObjects.Id methodId,
            ValueObjects.Id stakeholderId,
            ValueObjects.Timestamp timestamp
            )
          : base(timestamp)
        {
            MethodId = methodId;
            StakeholderId = stakeholderId;
        }

        public Task<Method> GetMethod(
            [DataLoader] MethodForTimestampedIdDataLoader methodLoader
            )
        {
            return methodLoader.LoadAsync(
                TimestampHelpers.TimestampId(MethodId, Timestamp)
                );
        }

        public Task<Stakeholder> GetStakeholder(
            [DataLoader] StakeholderForTimestampedIdDataLoader stakeholderLoader
            )
        {
            return stakeholderLoader.LoadAsync(
                TimestampHelpers.TimestampId(StakeholderId, Timestamp)
                );
        }
    }
}