using Infrastructure.Events;
using Guid = System.Guid;

namespace Metabase.Events
{
    public sealed class ComponentVersionRemoved
      : AssociationRemovedEvent
    {
        public static ComponentVersionRemoved From(
            Guid componentVersionId,
            Infrastructure.Commands.RemoveAssociationCommand<Infrastructure.ValueObjects.RemoveManyToManyAssociationInput<Models.ComponentVersion>> command
            )
        {
            return new ComponentVersionRemoved(
                componentVersionId: componentVersionId,
                creatorId: command.CreatorId
                );
        }

#nullable disable
        public ComponentVersionRemoved() { }
#nullable enable

        public ComponentVersionRemoved(
            Guid componentVersionId,
            Guid creatorId
            )
          : base(
              aggregateId: componentVersionId,
              creatorId: creatorId
              )
        {
            EnsureValid();
        }
    }
}