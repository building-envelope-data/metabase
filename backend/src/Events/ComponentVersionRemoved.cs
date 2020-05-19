using Errors = Icon.Errors;
using CSharpFunctionalExtensions;
using Guid = System.Guid;
using Commands = Icon.Commands;
using Newtonsoft.Json;

namespace Icon.Events
{
    public sealed class ComponentVersionRemoved
      : RemovedEvent
    {
        public static ComponentVersionRemoved From(
            Guid componentVersionId,
            Commands.Remove<ValueObjects.RemoveManyToManyAssociationInput<Models.ComponentVersion>> command
            )
        {
            return new ComponentVersionRemoved(
                componentVersionId: componentVersionId,
                baseComponentId: command.Input.ParentId,
                versionComponentId: command.Input.AssociateId,
                creatorId: command.CreatorId
                );
        }

        [JsonIgnore]
        public Guid BaseComponentId { get => ParentId; }

        [JsonIgnore]
        public Guid VersionComponentId { get => AssociateId; }

#nullable disable
        public ComponentVersionRemoved() { }
#nullable enable

        public ComponentVersionRemoved(
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