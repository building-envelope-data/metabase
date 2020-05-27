using Errors = Icon.Errors;
using CSharpFunctionalExtensions;
using Guid = System.Guid;
using Commands = Icon.Commands;
using Newtonsoft.Json;

namespace Icon.Events
{
    public sealed class ComponentVariantRemoved
      : AssociationRemovedEvent
    {
        public static ComponentVariantRemoved From(
            Guid componentVariantId,
            Commands.RemoveAssociation<ValueObjects.RemoveManyToManyAssociationInput<Models.ComponentVariant>> command
            )
        {
            return new ComponentVariantRemoved(
                componentVariantId: componentVariantId,
                creatorId: command.CreatorId
                );
        }

#nullable disable
        public ComponentVariantRemoved() { }
#nullable enable

        public ComponentVariantRemoved(
            Guid componentVariantId,
            Guid creatorId
            )
          : base(
              aggregateId: componentVariantId,
              creatorId: creatorId
              )
        {
            EnsureValid();
        }
    }
}