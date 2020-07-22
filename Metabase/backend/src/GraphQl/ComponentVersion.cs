using Infrastructure.GraphQl;
using Infrastructure.ValueObjects;
namespace Metabase.GraphQl
{
    public sealed class ComponentVersion
      : Node
    {
        public static ComponentVersion FromModel(
            Models.ComponentVersion model,
            Timestamp requestTimestamp
            )
        {
            return new ComponentVersion(
                id: model.Id,
                baseComponentId: model.BaseComponentId,
                versionComponentId: model.VersionComponentId,
                timestamp: model.Timestamp,
                requestTimestamp: requestTimestamp
                );
        }

        public Id BaseComponentId { get; }
        public Id VersionComponentId { get; }

        public ComponentVersion(
            Id id,
            Id baseComponentId,
            Id versionComponentId,
            Timestamp timestamp,
            Timestamp requestTimestamp
            )
          : base(
              id: id,
              timestamp: timestamp,
              requestTimestamp: requestTimestamp
              )
        {
            BaseComponentId = baseComponentId;
            VersionComponentId = versionComponentId;
        }
    }
}