using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Metabase.GraphQl
{
    public sealed class AddComponentConcretizationInput
      : AddOrRemoveComponentConcretizationInput
    {
        public AddComponentConcretizationInput(
            Id generalComponentId,
            Id concreteComponentId
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