using Errors = Icon.Errors;
using CSharpFunctionalExtensions;
using Guid = System.Guid;
using Commands = Icon.Commands;
using Newtonsoft.Json;

namespace Icon.Events
{
    public sealed class ComponentPartAdded
      : AddedEvent
    {
        public static ComponentPartAdded From(
            Guid componentPartId,
            Commands.Add<ValueObjects.AddComponentPartInput> command
            )
        {
            return new ComponentPartAdded(
                componentPartId: componentPartId,
                assembledComponentId: command.Input.AssembledComponentId,
                partComponentId: command.Input.PartComponentId,
                creatorId: command.CreatorId
                );
        }

        [JsonIgnore]
        public Guid AssembledComponentId { get => ParentId; set => ParentId = value; }

        [JsonIgnore]
        public Guid PartComponentId { get => AssociateId; set => AssociateId = value; }

#nullable disable
        public ComponentPartAdded() { }
#nullable enable

        public ComponentPartAdded(
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