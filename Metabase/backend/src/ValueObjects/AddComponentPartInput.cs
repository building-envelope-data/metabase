using CSharpFunctionalExtensions;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Metabase.ValueObjects
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
              Result.Success<AddComponentPartInput, Errors>(
                  new AddComponentPartInput(
                    assembledComponentId: assembledComponentId,
                    partComponentId: partComponentId
                    )
                  );
        }
    }
}