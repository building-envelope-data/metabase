using CSharpFunctionalExtensions;
using Infrastructure.Models;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Metabase.Models
{
    public sealed class ComponentVersion
      : Model, IManyToManyAssociation
    {
        public Id BaseComponentId { get; }
        public Id VersionComponentId { get; }

        public Id ParentId { get => BaseComponentId; }
        public Id AssociateId { get => VersionComponentId; }

        private ComponentVersion(
            Id id,
            Id baseComponentId,
            Id versionComponentId,
            Timestamp timestamp
            )
          : base(id, timestamp)
        {
            BaseComponentId = baseComponentId;
            VersionComponentId = versionComponentId;
        }

        public static Result<ComponentVersion, Errors> From(
            Id id,
            Id baseComponentId,
            Id versionComponentId,
            Timestamp timestamp
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