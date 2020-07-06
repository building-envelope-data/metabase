using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Metabase.GraphQl
{
    public sealed class RemoveComponentVariantInput
      : AddOrRemoveComponentVariantInput
    {
        public Timestamp Timestamp { get; }

        public RemoveComponentVariantInput(
            Id baseComponentId,
            Id variantComponentId,
            Timestamp timestamp
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