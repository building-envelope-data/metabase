using CSharpFunctionalExtensions;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Metabase.ValueObjects
{
    public sealed class AddComponentConcretizationInput
      : AddManyToManyAssociationInput
    {
        public Id GeneralComponentId { get => ParentId; }
        public Id ConcreteComponentId { get => AssociateId; }

        private AddComponentConcretizationInput(
            Id generalComponentId,
            Id concreteComponentId
            )
          : base(
              parentId: generalComponentId,
              associateId: concreteComponentId
              )
        {
        }

        public static Result<AddComponentConcretizationInput, Errors> From(
            Id generalComponentId,
            Id concreteComponentId
            )
        {
            return
              Result.Ok<AddComponentConcretizationInput, Errors>(
                  new AddComponentConcretizationInput(
                    generalComponentId: generalComponentId,
                    concreteComponentId: concreteComponentId
                    )
                  );
        }
    }
}