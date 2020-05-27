using System.Collections.Generic;
using System.Linq;
using ValueObjects = Icon.ValueObjects;
using Errors = Icon.Errors;
using CSharpFunctionalExtensions;

namespace Icon.GraphQl
{
    public sealed class AddComponentConcretizationInput
      : AddOrRemoveComponentConcretizationInput
    {
        public AddComponentConcretizationInput(
            ValueObjects.Id generalComponentId,
            ValueObjects.Id concreteComponentId
            )
          : base(
              generalComponentId: generalComponentId,
              concreteComponentId: concreteComponentId
              )
        {
        }

        public static Result<ValueObjects.AddComponentConcretizationInput, Errors> Validate(
            AddComponentConcretizationInput self,
            IReadOnlyList<object> path
            )
        {
            return ValueObjects.AddComponentConcretizationInput.From(
                    generalComponentId: self.GeneralComponentId,
                    concreteComponentId: self.ConcreteComponentId
                  );
        }
    }
}