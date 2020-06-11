using CSharpFunctionalExtensions;
using Newtonsoft.Json;
using Commands = Icon.Commands;
using Errors = Icon.Errors;
using Guid = System.Guid;

namespace Icon.Events
{
    public sealed class ComponentHygrothermalDataRemoved
      : AssociationRemovedEvent
    {
        public static ComponentHygrothermalDataRemoved From(
            Guid componentHygrothermalDataId,
            Commands.RemoveAssociation<ValueObjects.RemoveOneToManyAssociationInput<Models.ComponentHygrothermalData>> command
            )
        {
            return new ComponentHygrothermalDataRemoved(
                componentHygrothermalDataId: componentHygrothermalDataId,
                creatorId: command.CreatorId
                );
        }

#nullable disable
        public ComponentHygrothermalDataRemoved() { }
#nullable enable

        public ComponentHygrothermalDataRemoved(
            Guid componentHygrothermalDataId,
            Guid creatorId
            )
          : base(
              aggregateId: componentHygrothermalDataId,
              creatorId: creatorId
              )
        {
            EnsureValid();
        }
    }
}