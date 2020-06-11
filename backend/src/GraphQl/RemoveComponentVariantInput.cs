using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using Errors = Icon.Errors;
using ValueObjects = Icon.ValueObjects;

namespace Icon.GraphQl
{
    public sealed class RemoveComponentVariantInput
      : AddOrRemoveComponentVariantInput
    {
        public ValueObjects.Timestamp Timestamp { get; }

        public RemoveComponentVariantInput(
            ValueObjects.Id baseComponentId,
            ValueObjects.Id variantComponentId,
            ValueObjects.Timestamp timestamp
            )
          : base(
              baseComponentId: baseComponentId,
              variantComponentId: variantComponentId
              )
        {
            Timestamp = timestamp;
        }

        public static Result<ValueObjects.RemoveManyToManyAssociationInput<Models.ComponentVariant>, Errors> Validate(
            RemoveComponentVariantInput self,
            IReadOnlyList<object> path
            )
        {
            return ValueObjects.RemoveManyToManyAssociationInput<Models.ComponentVariant>.From(
                    parentId: self.BaseComponentId,
                    associateId: self.VariantComponentId,
                    timestamp: self.Timestamp
                  );
        }
    }
}