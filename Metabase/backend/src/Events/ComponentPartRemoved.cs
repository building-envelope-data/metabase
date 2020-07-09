using Infrastructure.Events;
using Guid = System.Guid;

namespace Metabase.Events
{
    public sealed class ComponentPartRemoved
      : AssociationRemovedEvent
    {
        public static ComponentPartRemoved From(
            Guid componentPartId,
            Infrastructure.Commands.RemoveAssociation<Infrastructure.ValueObjects.RemoveManyToManyAssociationInput<Models.ComponentPart>> command
            )
        {
            return new ComponentPartRemoved(
                componentPartId: componentPartId,
                creatorId: command.CreatorId
                );
        }

#nullable disable
        public ComponentPartRemoved() { }
#nullable enable

        public ComponentPartRemoved(
            Guid componentPartId,
            Guid creatorId
            )
          : base(
              aggregateId: componentPartId,
              creatorId: creatorId
              )
        {
            EnsureValid();
        }
    }
}