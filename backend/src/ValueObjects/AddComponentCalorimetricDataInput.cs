using System.Collections.Generic;
using CSharpFunctionalExtensions;
using DateTime = System.DateTime;
using Errors = Icon.Errors;

namespace Icon.ValueObjects
{
    public sealed class AddComponentCalorimetricDataInput
      : AddOneToManyAssociationInput
    {
        public static Result<AddComponentCalorimetricDataInput, Errors> From(
            Id componentId,
            Id calorimetricDataId
            )
        {
            return
              Result.Ok<AddComponentCalorimetricDataInput, Errors>(
                  new AddComponentCalorimetricDataInput(
                    componentId: componentId,
                    calorimetricDataId: calorimetricDataId
                    )
                  );
        }

        public Id ComponentId { get => ParentId; }
        public Id CalorimetricDataId { get => AssociateId; }

        private AddComponentCalorimetricDataInput(
            Id componentId,
            Id calorimetricDataId
            )
          : base(
              parentId: componentId,
              associateId: calorimetricDataId
              )
        {
        }
    }
}