using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Errors = Icon.Errors;

namespace Icon.ValueObjects
{
    public abstract class AddManyToManyAssociationInput
      : AddAssociationInput
    {
        protected AddManyToManyAssociationInput(
            Id parentId,
            Id associateId
            )
          : base(
              parentId: parentId,
              associateId: associateId
              )
        {
        }
    }
}