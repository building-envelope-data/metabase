using System.Collections.Generic;
using Errors = Icon.Errors;
using CSharpFunctionalExtensions;
using DateTime = System.DateTime;

namespace Icon.ValueObjects
{
    public sealed class AddComponentVariantInput
      : ValueObject
    {
        public Id BaseComponentId { get; }
        public Id VariantComponentId { get; }

        private AddComponentVariantInput(
            Id baseComponentId,
            Id variantComponentId
            )
        {
            BaseComponentId = baseComponentId;
            VariantComponentId = variantComponentId;
        }

        public static Result<AddComponentVariantInput, Errors> From(
            Id baseComponentId,
            Id variantComponentId
            )
        {
            return
              Result.Ok<AddComponentVariantInput, Errors>(
                  new AddComponentVariantInput(
                    baseComponentId: baseComponentId,
                    variantComponentId: variantComponentId
                    )
                  );
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return BaseComponentId;
            yield return VariantComponentId;
        }
    }
}