using Infrastructure.ValueObjects;
namespace Metabase.GraphQl
{
    public abstract class AddOrRemoveComponentVersionInput
    {
        public Id BaseComponentId { get; }
        public Id VersionComponentId { get; }

        protected AddOrRemoveComponentVersionInput(
            Id baseComponentId,
            Id versionComponentId
            )
        {
            BaseComponentId = baseComponentId;
            VersionComponentId = versionComponentId;
        }
    }
}