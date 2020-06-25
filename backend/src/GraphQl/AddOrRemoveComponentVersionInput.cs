namespace Icon.GraphQl
{
    public abstract class AddOrRemoveComponentVersionInput
    {
        public ValueObjects.Id BaseComponentId { get; }
        public ValueObjects.Id VersionComponentId { get; }

        protected AddOrRemoveComponentVersionInput(
            ValueObjects.Id baseComponentId,
            ValueObjects.Id versionComponentId
            )
        {
            BaseComponentId = baseComponentId;
            VersionComponentId = versionComponentId;
        }
    }
}