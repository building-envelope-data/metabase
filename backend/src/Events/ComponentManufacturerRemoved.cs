using Errors = Icon.Errors;
using CSharpFunctionalExtensions;
using Guid = System.Guid;
using Commands = Icon.Commands;
using Newtonsoft.Json;

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