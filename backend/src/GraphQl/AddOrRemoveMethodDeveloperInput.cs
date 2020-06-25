namespace Icon.GraphQl
{
    public abstract class AddOrRemoveMethodDeveloperInput
    {
        public ValueObjects.Id MethodId { get; }
        public ValueObjects.Id StakeholderId { get; }

        public AddOrRemoveMethodDeveloperInput(
            ValueObjects.Id methodId,
            ValueObjects.Id stakeholderId
            )
        {
            MethodId = methodId;
            StakeholderId = stakeholderId;
        }
    }
}