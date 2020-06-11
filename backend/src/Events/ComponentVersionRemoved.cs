using CSharpFunctionalExtensions;
using Newtonsoft.Json;
using Commands = Icon.Commands;
using Errors = Icon.Errors;
using Guid = System.Guid;

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