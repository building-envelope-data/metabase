using CSharpFunctionalExtensions;
using Newtonsoft.Json;
using Commands = Icon.Commands;
using Errors = Icon.Errors;
using Guid = System.Guid;

namespace Icon.Events
{
    public sealed class ComponentCalorimetricDataAdded
      : AssociationAddedEvent
    {
        public static ComponentCalorimetricDataAdded From(
            Guid componentCalorimetricDataId,
            Commands.AddAssociation<ValueObjects.AddComponentCalorimetricDataInput> command
            )
        {
            return new ComponentCalorimetricDataAdded(
                componentCalorimetricDataId: componentCalorimetricDataId,
                componentId: command.Input.ComponentId,
                calorimetricDataId: command.Input.CalorimetricDataId,
                creatorId: command.CreatorId
                );
        }

        public static ComponentCalorimetricDataAdded From(
            Guid componentCalorimetricDataId,
            Guid calorimetricDataId,
            Commands.Create<ValueObjects.CreateCalorimetricDataInput> command
            )
        {
            return new ComponentCalorimetricDataAdded(
                componentCalorimetricDataId: componentCalorimetricDataId,
                componentId: command.Input.ComponentId,
                calorimetricDataId: calorimetricDataId,
                creatorId: command.CreatorId
                );
        }

        [JsonIgnore]
        public Guid ComponentId { get => ParentId; }

        [JsonIgnore]
        public Guid CalorimetricDataId { get => AssociateId; }

#nullable disable
        public ComponentCalorimetricDataAdded() { }
#nullable enable

        public ComponentCalorimetricDataAdded(
            Guid componentCalorimetricDataId,
            Guid componentId,
            Guid calorimetricDataId,
            Guid creatorId
            )
          : base(
              aggregateId: componentCalorimetricDataId,
              parentId: componentId,
              associateId: calorimetricDataId,
              creatorId: creatorId
              )
        {
            EnsureValid();
        }
    }
}