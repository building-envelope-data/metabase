using System.Collections.Generic;
using System.Linq;
using ValueObjects = Icon.ValueObjects;
using Errors = Icon.Errors;
using CSharpFunctionalExtensions;

namespace Icon.GraphQl
{
    public abstract class AddOrRemoveComponentVariantInput
    {
        public ValueObjects.Id BaseComponentId { get; }
        public ValueObjects.Id VariantComponentId { get; }

        protected AddOrRemoveComponentVariantInput(
            ValueObjects.Id baseComponentId,
            ValueObjects.Id variantComponentId
            )
        {
            BaseComponentId = baseComponentId;
            VariantComponentId = variantComponentId;
        }
    }
}