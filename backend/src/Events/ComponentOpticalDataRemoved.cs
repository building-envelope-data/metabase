using CSharpFunctionalExtensions;
using Newtonsoft.Json;
using Commands = Icon.Commands;
using Errors = Icon.Errors;
using Guid = System.Guid;

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