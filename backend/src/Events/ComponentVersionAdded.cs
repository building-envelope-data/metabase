using Errors = Icon.Errors;
using CSharpFunctionalExtensions;
using Guid = System.Guid;
using Commands = Icon.Commands;
using Newtonsoft.Json;

namespace Icon.Events
{
    public sealed class ComponentVersionAdded
      : AddedEvent
    {
        public static ComponentVersionAdded From(
            Guid componentVersionId,
            Commands.Add<ValueObjects.AddComponentVersionInput> command
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
        public Guid BaseComponentId { get => ParentId; set => ParentId = value; }

        [JsonIgnore]
        public Guid VersionComponentId { get => AssociateId; set => AssociateId = value; }

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