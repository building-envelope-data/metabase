using Errors = Icon.Errors;
using CSharpFunctionalExtensions;
using Guid = System.Guid;
using Commands = Icon.Commands;
using Newtonsoft.Json;

namespace Icon.Events
{
    public sealed class ComponentPartRemoved
      : RemovedEvent
    {
        public static ComponentPartRemoved From(
            Guid componentPartId,
            Commands.Remove<ValueObjects.RemoveManyToManyAssociationInput<Models.ComponentPart>> command
            )
        {
            return new ComponentPartRemoved(
                componentPartId: componentPartId,
                assembledComponentId: command.Input.ParentId,
                partComponentId: command.Input.AssociateId,
                creatorId: command.CreatorId
                );
        }

        [JsonIgnore]
        public Guid AssembledComponentId { get => ParentId; }

        [JsonIgnore]
        public Guid PartComponentId { get => AssociateId; }

#nullable disable
        public ComponentPartRemoved() { }
#nullable enable

        public ComponentPartRemoved(
            Guid componentPartId,
            Guid assembledComponentId,
            Guid partComponentId,
            Guid creatorId
            )
          : base(
              aggregateId: componentPartId,
              parentId: assembledComponentId,
              associateId: partComponentId,
              creatorId: creatorId
              )
        {
            EnsureValid();
        }
    }
}