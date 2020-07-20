using Infrastructure.Events;
using Newtonsoft.Json;
using Guid = System.Guid;

namespace Metabase.Events
{
    public sealed class ComponentVersionAdded
      : AssociationAddedEvent
    {
        public static ComponentVersionAdded From(
            Guid componentVersionId,
            Infrastructure.Commands.AddAssociationCommand<ValueObjects.AddComponentVersionInput> command
            )
        {
            return new ComponentVersionAdded(
                componentVersionId: componentVersionId,
                baseComponentId: command.Input.BaseComponentId,
                versionComponentId: command.Input.VersionComponentId,
                creatorId: command.CreatorId
                );
        }

        [JsonIgnore]
        public Guid BaseComponentId { get => ParentId; }

        [JsonIgnore]
        public Guid VersionComponentId { get => AssociateId; }

#nullable disable
        public ComponentVersionAdded() { }
#nullable enable

        public ComponentVersionAdded(
            Guid componentVersionId,
            Guid baseComponentId,
            Guid versionComponentId,
            Guid creatorId
            )
          : base(
              aggregateId: componentVersionId,
              parentId: baseComponentId,
              associateId: versionComponentId,
              creatorId: creatorId
              )
        {
            EnsureValid();
        }
    }
}