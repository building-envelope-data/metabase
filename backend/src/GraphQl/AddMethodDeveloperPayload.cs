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
            MethodDeveloper methodDeveloper
            )
          : base(methodDeveloper.RequestTimestamp)
        {
            MethodId = methodDeveloper.MethodId;
            StakeholderId = methodDeveloper.StakeholderId;
        }

        public Task<Method> GetMethod(
            [DataLoader] MethodForTimestampedIdDataLoader methodLoader
            )
        {
            return methodLoader.LoadAsync(
                TimestampHelpers.TimestampId(MethodId, RequestTimestamp)
                );
        }

        public Task<Stakeholder> GetStakeholder(
            [DataLoader] StakeholderForTimestampedIdDataLoader stakeholderLoader
            )
        {
            return stakeholderLoader.LoadAsync(
                TimestampHelpers.TimestampId(StakeholderId, RequestTimestamp)
                );
        }
    }
}