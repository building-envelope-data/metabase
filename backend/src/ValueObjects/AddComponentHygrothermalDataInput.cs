using System.Collections.Generic;
using CSharpFunctionalExtensions;
using DateTime = System.DateTime;
using Errors = Icon.Errors;

namespace Icon.ValueObjects
{
    public sealed class AddComponentHygrothermalDataInput
      : AddOneToManyAssociationInput
    {
        public static Result<AddComponentHygrothermalDataInput, Errors> From(
            Id componentId,
            Id hygrothermalDataId
            )
        {
            return
              Result.Ok<AddComponentHygrothermalDataInput, Errors>(
                  new AddComponentHygrothermalDataInput(
                    componentId: componentId,
                    hygrothermalDataId: hygrothermalDataId
                    )
                  );
        }

        public Id ComponentId { get => ParentId; }
        public Id HygrothermalDataId { get => AssociateId; }

        private AddComponentHygrothermalDataInput(
            Id componentId,
            Id hygrothermalDataId
            )
          : base(
              parentId: componentId,
              associateId: hygrothermalDataId
              )
        {
        }
    }
}