using System.Collections.Generic;
using CSharpFunctionalExtensions;
using DateTime = System.DateTime;
using Errors = Icon.Errors;

namespace Icon.ValueObjects
{
    public sealed class AddComponentPartInput
      : AddManyToManyAssociationInput
    {
        public Id AssembledComponentId { get => ParentId; }
        public Id PartComponentId { get => AssociateId; }

        private AddComponentPartInput(
            Id assembledComponentId,
            Id partComponentId
            )
          : base(
              parentId: assembledComponentId,
              associateId: partComponentId
              )
        {
        }

        public static Result<AddComponentPartInput, Errors> From(
            Id assembledComponentId,
            Id partComponentId
            )
        {
            return
              Result.Ok<AddComponentPartInput, Errors>(
                  new AddComponentPartInput(
                    assembledComponentId: assembledComponentId,
                    partComponentId: partComponentId
                    )
                  );
        }
    }
}