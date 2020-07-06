using Icon.Infrastructure.Events;
using Guid = System.Guid;

namespace Icon.Events
{
    public sealed class ComponentManufacturerRemoved
      : AssociationRemovedEvent
    {
        public static ComponentManufacturerRemoved From(
              Guid componentManufacturerId,
              Commands.RemoveAssociation<ValueObjects.RemoveManyToManyAssociationInput<Models.ComponentManufacturer>> command
            )
        {
            return new ComponentManufacturerRemoved(
                componentManufacturerId: componentManufacturerId,
                creatorId: command.CreatorId
                );
        }

#nullable disable
        public ComponentManufacturerRemoved() { }
#nullable enable

        public ComponentManufacturerRemoved(
            Guid componentManufacturerId,
            Guid creatorId
            )
          : base(
              aggregateId: componentManufacturerId,
              creatorId: creatorId
              )
        {
            EnsureValid();
        }
    }
}