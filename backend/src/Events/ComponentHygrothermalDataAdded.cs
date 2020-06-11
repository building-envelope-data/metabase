using CSharpFunctionalExtensions;
using Newtonsoft.Json;
using Commands = Icon.Commands;
using Errors = Icon.Errors;
using Guid = System.Guid;

namespace Icon.Events
{
    public sealed class ComponentHygrothermalDataAdded
      : AssociationAddedEvent
    {
        public static ComponentHygrothermalDataAdded From(
            Guid componentHygrothermalDataId,
            Commands.AddAssociation<ValueObjects.AddComponentHygrothermalDataInput> command
            )
        {
            return new ComponentHygrothermalDataAdded(
                componentHygrothermalDataId: componentHygrothermalDataId,
                componentId: command.Input.ComponentId,
                hygrothermalDataId: command.Input.HygrothermalDataId,
                creatorId: command.CreatorId
                );
        }

        public static ComponentHygrothermalDataAdded From(
            Guid componentHygrothermalDataId,
            Guid hygrothermalDataId,
            Commands.Create<ValueObjects.CreateHygrothermalDataInput> command
            )
        {
            return new ComponentHygrothermalDataAdded(
                componentHygrothermalDataId: componentHygrothermalDataId,
                componentId: command.Input.ComponentId,
                hygrothermalDataId: hygrothermalDataId,
                creatorId: command.CreatorId
                );
        }

        [JsonIgnore]
        public Guid ComponentId { get => ParentId; }

        [JsonIgnore]
        public Guid HygrothermalDataId { get => AssociateId; }

#nullable disable
        public ComponentHygrothermalDataAdded() { }
#nullable enable

        public ComponentHygrothermalDataAdded(
            Guid componentHygrothermalDataId,
            Guid componentId,
            Guid hygrothermalDataId,
            Guid creatorId
            )
          : base(
              aggregateId: componentHygrothermalDataId,
              parentId: componentId,
              associateId: hygrothermalDataId,
              creatorId: creatorId
              )
        {
            EnsureValid();
        }
    }
}