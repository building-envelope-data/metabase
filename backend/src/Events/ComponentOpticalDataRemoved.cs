using Errors = Icon.Errors;
using CSharpFunctionalExtensions;
using Guid = System.Guid;
using Commands = Icon.Commands;
using Newtonsoft.Json;

namespace Icon.Events
{
    public sealed class ComponentOpticalDataRemoved
      : AssociationRemovedEvent
    {
        public static ComponentOpticalDataRemoved From(
            Guid componentOpticalDataId,
            Commands.RemoveAssociation<ValueObjects.RemoveManyToManyAssociationInput<Models.ComponentOpticalData>> command
            )
        {
            return new ComponentOpticalDataRemoved(
                componentOpticalDataId: componentOpticalDataId,
                creatorId: command.CreatorId
                );
        }

#nullable disable
        public ComponentOpticalDataRemoved() { }
#nullable enable

        public ComponentOpticalDataRemoved(
            Guid componentOpticalDataId,
            Guid creatorId
            )
          : base(
              aggregateId: componentOpticalDataId,
              creatorId: creatorId
              )
        {
            EnsureValid();
        }
    }
}