using System.Collections.Generic;
using System.Linq;
using ValueObjects = Icon.ValueObjects;
using Errors = Icon.Errors;
using CSharpFunctionalExtensions;

namespace Icon.GraphQl
{
    public sealed class AddComponentVariantInput
      : AddOrRemoveComponentVariantInput
    {
        public AddComponentVariantInput(
            ValueObjects.Id baseComponentId,
            ValueObjects.Id variantComponentId
            )
          : base(
              baseComponentId: baseComponentId,
              variantComponentId: variantComponentId
              )
        {
        }

        public static Result<ValueObjects.AddComponentVariantInput, Errors> Validate(
            AddComponentVariantInput self,
            IReadOnlyList<object> path
            )
        {
            return ValueObjects.AddComponentVariantInput.From(
                    baseComponentId: self.BaseComponentId,
                    variantComponentId: self.VariantComponentId
                  );
        }
    }
}