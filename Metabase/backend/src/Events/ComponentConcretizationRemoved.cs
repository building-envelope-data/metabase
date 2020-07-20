using Infrastructure.Events;
using Guid = System.Guid;

namespace Metabase.Events
{
    public sealed class ComponentConcretizationRemoved
      : AssociationRemovedEvent
    {
        public static ComponentConcretizationRemoved From(
            Guid componentConcretizationId,
            Infrastructure.Commands.RemoveAssociationCommand<Infrastructure.ValueObjects.RemoveManyToManyAssociationInput<Models.ComponentConcretization>> command
            )
        {
            return new ComponentConcretizationRemoved(
                componentConcretizationId: componentConcretizationId,
                creatorId: command.CreatorId
                );
        }

#nullable disable
        public ComponentConcretizationRemoved() { }
#nullable enable

        public ComponentConcretizationRemoved(
            Guid componentConcretizationId,
            Guid creatorId
            )
          : base(
              aggregateId: componentConcretizationId,
              creatorId: creatorId
              )
        {
            EnsureValid();
        }
    }
}