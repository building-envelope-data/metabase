using CSharpFunctionalExtensions;
using Newtonsoft.Json;
using Commands = Icon.Commands;
using Errors = Icon.Errors;
using Guid = System.Guid;

namespace Icon.Events
{
    public sealed class ComponentCalorimetricDataRemoved
      : AssociationRemovedEvent
    {
        public static ComponentCalorimetricDataRemoved From(
            Guid componentCalorimetricDataId,
            Commands.RemoveAssociation<ValueObjects.RemoveOneToManyAssociationInput<Models.ComponentCalorimetricData>> command
            )
        {
            return new ComponentCalorimetricDataRemoved(
                componentCalorimetricDataId: componentCalorimetricDataId,
                creatorId: command.CreatorId
                );
        }

#nullable disable
        public ComponentCalorimetricDataRemoved() { }
#nullable enable

        public ComponentCalorimetricDataRemoved(
            Guid componentCalorimetricDataId,
            Guid creatorId
            )
          : base(
              aggregateId: componentCalorimetricDataId,
              creatorId: creatorId
              )
        {
            EnsureValid();
        }
    }
}