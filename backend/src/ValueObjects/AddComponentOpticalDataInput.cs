using System.Collections.Generic;
using CSharpFunctionalExtensions;
using DateTime = System.DateTime;
using Errors = Icon.Errors;

namespace Icon.ValueObjects
{
    public sealed class AddComponentOpticalDataInput
      : AddManyToManyAssociationInput
    {
        public Id ComponentId { get => ParentId; }
        public Id OpticalDataId { get => AssociateId; }

        private AddComponentOpticalDataInput(
            Id componentId,
            Id opticalDataId
            )
          : base(
              parentId: componentId,
              associateId: opticalDataId
              )
        {
        }

        public static Result<AddComponentOpticalDataInput, Errors> From(
            Id componentId,
            Id opticalDataId
            )
        {
            return
              Result.Ok<AddComponentOpticalDataInput, Errors>(
                  new AddComponentOpticalDataInput(
                    componentId: componentId,
                    opticalDataId: opticalDataId
                    )
                  );
        }
    }
}