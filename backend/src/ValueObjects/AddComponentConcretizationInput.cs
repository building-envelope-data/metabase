using System.Collections.Generic;
using Errors = Icon.Errors;
using CSharpFunctionalExtensions;
using DateTime = System.DateTime;

namespace Icon.ValueObjects
{
    public sealed class AddComponentConcretizationInput
      : ValueObject
    {
        public Id GeneralComponentId { get; }
        public Id ConcreteComponentId { get; }

        private AddComponentConcretizationInput(
            Id generalComponentId,
            Id concreteComponentId
            )
        {
            GeneralComponentId = generalComponentId;
            ConcreteComponentId = concreteComponentId;
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

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return GeneralComponentId;
            yield return ConcreteComponentId;
        }
    }
}