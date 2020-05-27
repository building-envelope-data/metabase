using Errors = Icon.Errors;
using CSharpFunctionalExtensions;
using Guid = System.Guid;
using Commands = Icon.Commands;
using Newtonsoft.Json;

namespace Icon.Events
{
    public sealed class ComponentVersionRemoved
      : AssociationRemovedEvent
    {
        public static ComponentVersionRemoved From(
            Guid componentVersionId,
            Commands.RemoveAssociation<ValueObjects.RemoveManyToManyAssociationInput<Models.ComponentVersion>> command
            )
        {
            return new ComponentVersionRemoved(
                componentVersionId: componentVersionId,
                creatorId: command.CreatorId
                );
        }

#nullable disable
        public ComponentVersionRemoved() { }
#nullable enable

        public ComponentVersionRemoved(
            Guid componentVersionId,
            Guid creatorId
            )
          : base(
              aggregateId: componentVersionId,
              creatorId: creatorId
              )
        {
            EnsureValid();
        }
    }
}