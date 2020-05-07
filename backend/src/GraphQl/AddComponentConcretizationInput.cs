using System.Collections.Generic;
using System.Linq;
using ValueObjects = Icon.ValueObjects;
using Errors = Icon.Errors;
using CSharpFunctionalExtensions;

namespace Icon.GraphQl
{
    public sealed class AddComponentConcretizationInput
    {
        public ValueObjects.Id GeneralComponentId { get; }
        public ValueObjects.Id ConcreteComponentId { get; }

        private AddComponentConcretizationInput(
            ValueObjects.Id generalComponentId,
            ValueObjects.Id concreteComponentId
            )
        {
            GeneralComponentId = generalComponentId;
            ConcreteComponentId = concreteComponentId;
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