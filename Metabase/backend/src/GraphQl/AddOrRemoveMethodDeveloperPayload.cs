using System.Threading.Tasks;
using GreenDonut;
using HotChocolate;
using Infrastructure.GraphQl;
using Infrastructure.ValueObjects;

namespace Metabase.GraphQl
{
    public abstract class AddOrRemoveMethodDeveloperPayload
      : Payload
    {
        public Id MethodId { get; }
        public Id StakeholderId { get; }

        protected AddOrRemoveMethodDeveloperPayload(
            Id methodId,
            Id stakeholderId,
            Timestamp requestTimestamp
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

        public Task<IStakeholder> GetStakeholder(
            [DataLoader] StakeholderDataLoader stakeholderLoader
            )
        {
            return stakeholderLoader.LoadAsync(
                TimestampHelpers.TimestampId(StakeholderId, RequestTimestamp)
                );
        }
    }
}