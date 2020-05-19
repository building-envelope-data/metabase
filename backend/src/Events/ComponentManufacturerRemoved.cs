using Errors = Icon.Errors;
using CSharpFunctionalExtensions;
using Guid = System.Guid;
using Commands = Icon.Commands;
using Newtonsoft.Json;

namespace Icon.Events
{
    public sealed class ComponentManufacturerRemoved
      : RemovedEvent
    {
        public static ComponentManufacturerRemoved From(
              Guid componentManufacturerId,
              Commands.Remove<ValueObjects.RemoveManyToManyAssociationInput<Models.ComponentManufacturer>> command
            )
        {
            return new ComponentManufacturerRemoved(
                componentManufacturerId: componentManufacturerId,
                componentId: command.Input.ParentId,
                institutionId: command.Input.AssociateId,
                creatorId: command.CreatorId
                );
        }

        [JsonIgnore]
        public Guid ComponentId { get => ParentId; }

        [JsonIgnore]
        public Guid InstitutionId { get => AssociateId; }

#nullable disable
        public ComponentManufacturerRemoved() { }
#nullable enable

        public ComponentManufacturerRemoved(
            Guid componentManufacturerId,
            Guid componentId,
            Guid institutionId,
            Guid creatorId
            )
          : base(
              aggregateId: componentManufacturerId,
              parentId: componentId,
              associateId: institutionId,
              creatorId: creatorId
              )
        {
            EnsureValid();
        }
    }
}