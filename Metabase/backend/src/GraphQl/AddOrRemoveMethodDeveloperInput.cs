using Infrastructure.ValueObjects;
namespace Metabase.GraphQl
{
    public abstract class AddOrRemoveMethodDeveloperInput
    {
        public Id MethodId { get; }
        public Id StakeholderId { get; }

        public AddOrRemoveMethodDeveloperInput(
            Id methodId,
            Id stakeholderId
            )
        {
            MethodId = methodId;
            StakeholderId = stakeholderId;
        }
    }
}