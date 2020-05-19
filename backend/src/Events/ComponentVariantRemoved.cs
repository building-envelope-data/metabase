using Errors = Icon.Errors;
using CSharpFunctionalExtensions;
using Guid = System.Guid;
using Commands = Icon.Commands;
using Newtonsoft.Json;

namespace Icon.Events
{
    public sealed class ComponentVariantRemoved
      : RemovedEvent
    {
        public static ComponentVariantRemoved From(
            Guid componentVariantId,
            Commands.Remove<ValueObjects.RemoveManyToManyAssociationInput<Models.ComponentVariant>> command
            )
        {
            return new ComponentVariantRemoved(
                componentVariantId: componentVariantId,
                baseComponentId: command.Input.ParentId,
                variantComponentId: command.Input.AssociateId,
                creatorId: command.CreatorId
                );
        }

        [JsonIgnore]
        public Guid BaseComponentId { get => ParentId; }

        [JsonIgnore]
        public Guid VariantComponentId { get => AssociateId; }

#nullable disable
        public ComponentVariantRemoved() { }
#nullable enable

        public ComponentVariantRemoved(
            Guid componentVariantId,
            Guid baseComponentId,
            Guid variantComponentId,
            Guid creatorId
            )
          : base(
              aggregateId: componentVariantId,
              parentId: baseComponentId,
              associateId: variantComponentId,
              creatorId: creatorId
              )
        {
            EnsureValid();
        }
    }
}