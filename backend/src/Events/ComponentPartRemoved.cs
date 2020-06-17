using CSharpFunctionalExtensions;
using Newtonsoft.Json;
using Commands = Icon.Commands;
using Errors = Icon.Errors;
using Guid = System.Guid;

namespace Icon.Events
{
    public sealed class ComponentPartRemoved
      : AssociationRemovedEvent
    {
        public static ComponentPartRemoved From(
            Guid componentPartId,
            Commands.RemoveAssociation<ValueObjects.RemoveManyToManyAssociationInput<Models.ComponentPart>> command
            )
        {
            return new ComponentPartRemoved(
                componentPartId: componentPartId,
                creatorId: command.CreatorId
                );
        }

#nullable disable
        public ComponentPartRemoved() { }
#nullable enable

        public ComponentPartRemoved(
            Guid componentPartId,
            Guid creatorId
            )
          : base(
              aggregateId: componentPartId,
              creatorId: creatorId
              )
        {
            EnsureValid();
        }
    }
}