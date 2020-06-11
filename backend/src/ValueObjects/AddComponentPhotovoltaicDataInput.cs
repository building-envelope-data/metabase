using System.Collections.Generic;
using CSharpFunctionalExtensions;
using DateTime = System.DateTime;
using Errors = Icon.Errors;

namespace Icon.ValueObjects
{
    public sealed class AddComponentPhotovoltaicDataInput
      : AddOneToManyAssociationInput
    {
        public static Result<AddComponentPhotovoltaicDataInput, Errors> From(
            Id componentId,
            Id photovoltaicDataId
            )
        {
            return
              Result.Ok<AddComponentPhotovoltaicDataInput, Errors>(
                  new AddComponentPhotovoltaicDataInput(
                    componentId: componentId,
                    photovoltaicDataId: photovoltaicDataId
                    )
                  );
        }

        public Id ComponentId { get => ParentId; }
        public Id PhotovoltaicDataId { get => AssociateId; }

        private AddComponentPhotovoltaicDataInput(
            Id componentId,
            Id photovoltaicDataId
            )
          : base(
              parentId: componentId,
              associateId: photovoltaicDataId
              )
        {
        }
    }
}