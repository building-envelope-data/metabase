using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Metabase.GraphQl
{
    public sealed class AddComponentVariantInput
      : AddOrRemoveComponentVariantInput
    {
        public AddComponentVariantInput(
            Id baseComponentId,
            Id variantComponentId
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