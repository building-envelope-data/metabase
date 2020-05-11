using ValueObjects = Icon.ValueObjects;
using Errors = Icon.Errors;
using CSharpFunctionalExtensions;

namespace Icon.Models
{
    public sealed class ComponentVariant
      : Model
    {
        public ValueObjects.Id BaseComponentId { get; }
        public ValueObjects.Id VariantComponentId { get; }

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