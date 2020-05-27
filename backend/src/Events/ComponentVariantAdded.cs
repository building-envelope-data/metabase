using Errors = Icon.Errors;
using CSharpFunctionalExtensions;
using Guid = System.Guid;
using Commands = Icon.Commands;
using Newtonsoft.Json;

namespace Icon.Events
{
    public sealed class ComponentVariantAdded
      : AssociationAddedEvent
    {
        public static ComponentVariantAdded From(
            Guid componentVariantId,
            Commands.AddAssociation<ValueObjects.AddComponentVariantInput> command
            )
        {
            return new ComponentVariantAdded(
                componentVariantId: componentVariantId,
                baseComponentId: command.Input.BaseComponentId,
                variantComponentId: command.Input.VariantComponentId,
                creatorId: command.CreatorId
                );
        }

        [JsonIgnore]
        public Guid BaseComponentId { get => ParentId; }

        [JsonIgnore]
        public Guid VariantComponentId { get => AssociateId; }

#nullable disable
        public ComponentVariantAdded() { }
#nullable enable

        public ComponentVariantAdded(
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