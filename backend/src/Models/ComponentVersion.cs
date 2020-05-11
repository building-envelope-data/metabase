using ValueObjects = Icon.ValueObjects;
using Errors = Icon.Errors;
using CSharpFunctionalExtensions;

namespace Icon.Models
{
    public sealed class ComponentVersion
      : Model
    {
        public ValueObjects.Id BaseComponentId { get; }
        public ValueObjects.Id VersionComponentId { get; }

        private ComponentVersion(
            ValueObjects.Id id,
            ValueObjects.Id baseComponentId,
            ValueObjects.Id versionComponentId,
            ValueObjects.Timestamp timestamp
            )
          : base(id, timestamp)
        {
            BaseComponentId = baseComponentId;
            VersionComponentId = versionComponentId;
        }

        public static Result<ComponentVersion, Errors> From(
            ValueObjects.Id id,
            ValueObjects.Id baseComponentId,
            ValueObjects.Id versionComponentId,
            ValueObjects.Timestamp timestamp
            )
        {
            return
              Result.Ok<ComponentVersion, Errors>(
                  new ComponentVersion(
            id: id,
            baseComponentId: baseComponentId,
            versionComponentId: versionComponentId,
            timestamp: timestamp
            )
                  );
        }
    }
}