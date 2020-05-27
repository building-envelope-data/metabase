using Errors = Icon.Errors;
using CSharpFunctionalExtensions;
using Guid = System.Guid;
using Commands = Icon.Commands;
using Newtonsoft.Json;

namespace Icon.Events
{
    public sealed class ComponentPartRemoved
      : AssociationRemovedEvent
    {
        public static ComponentPartRemoved From(
            Guid componentPartId,
            Commands.Remove<ValueObjects.RemoveManyToManyAssociationInput<Models.ComponentPart>> command
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