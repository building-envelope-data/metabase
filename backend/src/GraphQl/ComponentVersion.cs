using Models = Icon.Models;
using DateTime = System.DateTime;

namespace Icon.GraphQl
{
    public sealed class ComponentVersion
      : NodeBase
    {
        public static ComponentVersion FromModel(
            Models.ComponentVersion model,
            ValueObjects.Timestamp requestTimestamp
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

        public ValueObjects.Id BaseComponentId { get; }
        public ValueObjects.Id VersionComponentId { get; }

        public ComponentVersion(
            ValueObjects.Id id,
            ValueObjects.Id baseComponentId,
            ValueObjects.Id versionComponentId,
            ValueObjects.Timestamp timestamp,
            ValueObjects.Timestamp requestTimestamp
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