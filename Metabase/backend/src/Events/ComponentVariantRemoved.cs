using Infrastructure.Events;
using Guid = System.Guid;

namespace Metabase.Events
{
    public sealed class ComponentVariantRemoved
      : AssociationRemovedEvent
    {
        public static ComponentVariantRemoved From(
            Guid componentVariantId,
            Infrastructure.Commands.RemoveAssociationCommand<Infrastructure.ValueObjects.RemoveManyToManyAssociationInput<Models.ComponentVariant>> command
            )
        {
            return new ComponentVariantRemoved(
                componentVariantId: componentVariantId,
                creatorId: command.CreatorId
                );
        }

#nullable disable
        public ComponentVariantRemoved() { }
#nullable enable

        public ComponentVariantRemoved(
            Guid componentVariantId,
            Guid creatorId
            )
          : base(
              aggregateId: componentVariantId,
              creatorId: creatorId
              )
        {
            EnsureValid();
        }
    }
}