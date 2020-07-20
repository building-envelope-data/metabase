using Infrastructure.Events;
using Newtonsoft.Json;
using Guid = System.Guid;

namespace Metabase.Events
{
    public sealed class ComponentPartAdded
      : AssociationAddedEvent
    {
        public static ComponentPartAdded From(
            Guid componentPartId,
            Infrastructure.Commands.AddAssociationCommand<ValueObjects.AddComponentPartInput> command
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
        public Guid AssembledComponentId { get => ParentId; }

        [JsonIgnore]
        public Guid PartComponentId { get => AssociateId; }

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