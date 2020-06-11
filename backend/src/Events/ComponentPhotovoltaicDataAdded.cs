using CSharpFunctionalExtensions;
using Newtonsoft.Json;
using Commands = Icon.Commands;
using Errors = Icon.Errors;
using Guid = System.Guid;

namespace Icon.Events
{
    public sealed class ComponentPhotovoltaicDataAdded
      : AssociationAddedEvent
    {
        public static ComponentPhotovoltaicDataAdded From(
            Guid componentPhotovoltaicDataId,
            Commands.AddAssociation<ValueObjects.AddComponentPhotovoltaicDataInput> command
            )
        {
            return new ComponentPhotovoltaicDataAdded(
                componentPhotovoltaicDataId: componentPhotovoltaicDataId,
                componentId: command.Input.ComponentId,
                photovoltaicDataId: command.Input.PhotovoltaicDataId,
                creatorId: command.CreatorId
                );
        }

        public static ComponentPhotovoltaicDataAdded From(
            Guid componentPhotovoltaicDataId,
            Guid photovoltaicDataId,
            Commands.Create<ValueObjects.CreatePhotovoltaicDataInput> command
            )
        {
            return new ComponentPhotovoltaicDataAdded(
                componentPhotovoltaicDataId: componentPhotovoltaicDataId,
                componentId: command.Input.ComponentId,
                photovoltaicDataId: photovoltaicDataId,
                creatorId: command.CreatorId
                );
        }

        [JsonIgnore]
        public Guid ComponentId { get => ParentId; }

        [JsonIgnore]
        public Guid PhotovoltaicDataId { get => AssociateId; }

#nullable disable
        public ComponentPhotovoltaicDataAdded() { }
#nullable enable

        public ComponentPhotovoltaicDataAdded(
            Guid componentPhotovoltaicDataId,
            Guid componentId,
            Guid photovoltaicDataId,
            Guid creatorId
            )
          : base(
              aggregateId: componentPhotovoltaicDataId,
              parentId: componentId,
              associateId: photovoltaicDataId,
              creatorId: creatorId
              )
        {
            EnsureValid();
        }
    }
}