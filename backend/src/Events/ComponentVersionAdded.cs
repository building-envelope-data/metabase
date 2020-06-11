using CSharpFunctionalExtensions;
using Newtonsoft.Json;
using Commands = Icon.Commands;
using Errors = Icon.Errors;
using Guid = System.Guid;

namespace Icon.Events
{
    public sealed class ComponentVersionAdded
      : AssociationAddedEvent
    {
        public static ComponentVersionAdded From(
            Guid componentVersionId,
            Commands.AddAssociation<ValueObjects.AddComponentVersionInput> command
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