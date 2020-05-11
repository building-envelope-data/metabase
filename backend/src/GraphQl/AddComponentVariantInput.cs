using System.Collections.Generic;
using System.Linq;
using ValueObjects = Icon.ValueObjects;
using Errors = Icon.Errors;
using CSharpFunctionalExtensions;

namespace Icon.GraphQl
{
    public sealed class AddComponentVariantInput
    {
        public ValueObjects.Id BaseComponentId { get; }
        public ValueObjects.Id VariantComponentId { get; }

        private AddComponentVariantInput(
            ValueObjects.Id baseComponentId,
            ValueObjects.Id variantComponentId
            )
        {
            BaseComponentId = baseComponentId;
            VariantComponentId = variantComponentId;
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