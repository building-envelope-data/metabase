using System.Collections.Generic;
using CSharpFunctionalExtensions;
using DateTime = System.DateTime;
using Errors = Icon.Errors;

namespace Icon.ValueObjects
{
    public sealed class AddComponentVariantInput
      : AddManyToManyAssociationInput
    {
        public Id BaseComponentId { get => ParentId; }
        public Id VariantComponentId { get => AssociateId; }

        private AddComponentVariantInput(
            Id baseComponentId,
            Id variantComponentId
            )
          : base(
              parentId: baseComponentId,
              associateId: variantComponentId
              )
        {
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
    }
}