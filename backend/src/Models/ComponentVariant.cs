using CSharpFunctionalExtensions;
using Errors = Icon.Errors;
using ValueObjects = Icon.ValueObjects;

namespace Icon.Models
{
    public sealed class ComponentVariant
      : Model, IManyToManyAssociation
    {
        public ValueObjects.Id BaseComponentId { get; }
        public ValueObjects.Id VariantComponentId { get; }

        public ValueObjects.Id ParentId { get => BaseComponentId; }
        public ValueObjects.Id AssociateId { get => VariantComponentId; }

        private ComponentVariant(
            ValueObjects.Id id,
            ValueObjects.Id baseComponentId,
            ValueObjects.Id variantComponentId,
            ValueObjects.Timestamp timestamp
            )
          : base(id, timestamp)
        {
            BaseComponentId = baseComponentId;
            VariantComponentId = variantComponentId;
        }

        public static Result<ComponentVariant, Errors> From(
            ValueObjects.Id id,
            ValueObjects.Id baseComponentId,
            ValueObjects.Id variantComponentId,
            ValueObjects.Timestamp timestamp
            )
        {
            return
              Result.Ok<ComponentVariant, Errors>(
                  new ComponentVariant(
            id: id,
            baseComponentId: baseComponentId,
            variantComponentId: variantComponentId,
            timestamp: timestamp
            )
                  );
        }
    }
}