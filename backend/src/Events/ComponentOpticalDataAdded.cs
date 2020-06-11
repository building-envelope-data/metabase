using CSharpFunctionalExtensions;
using Newtonsoft.Json;
using Commands = Icon.Commands;
using Errors = Icon.Errors;
using Guid = System.Guid;

namespace Icon.Events
{
    public sealed class ComponentOpticalDataAdded
      : AssociationAddedEvent
    {
        public static ComponentOpticalDataAdded From(
            Guid componentOpticalDataId,
            Commands.AddAssociation<ValueObjects.AddComponentOpticalDataInput> command
            )
        {
            return new ComponentOpticalDataAdded(
                componentOpticalDataId: componentOpticalDataId,
                componentId: command.Input.ComponentId,
                opticalDataId: command.Input.OpticalDataId,
                creatorId: command.CreatorId
                );
        }

        public static ComponentOpticalDataAdded From(
            Guid componentOpticalDataId,
            Guid opticalDataId,
            Commands.Create<ValueObjects.CreateOpticalDataInput> command
            )
        {
            return new ComponentOpticalDataAdded(
                componentOpticalDataId: componentOpticalDataId,
                componentId: command.Input.ComponentId,
                opticalDataId: opticalDataId,
                creatorId: command.CreatorId
                );
        }

        [JsonIgnore]
        public Guid ComponentId { get => ParentId; }

        [JsonIgnore]
        public Guid OpticalDataId { get => AssociateId; }

#nullable disable
        public ComponentOpticalDataAdded() { }
#nullable enable

        public ComponentOpticalDataAdded(
            Guid componentOpticalDataId,
            Guid componentId,
            Guid opticalDataId,
            Guid creatorId
            )
          : base(
              aggregateId: componentOpticalDataId,
              parentId: componentId,
              associateId: opticalDataId,
              creatorId: creatorId
              )
        {
            EnsureValid();
        }
    }
}