using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace Icon.GraphQl
{
    public sealed class RemoveComponentConcretizationInput
      : AddOrRemoveComponentConcretizationInput
    {
        public ValueObjects.Timestamp Timestamp { get; }

        public RemoveComponentConcretizationInput(
            ValueObjects.Id generalComponentId,
            ValueObjects.Id concreteComponentId,
            ValueObjects.Timestamp timestamp
            )
          : base(
              generalComponentId: generalComponentId,
              concreteComponentId: concreteComponentId
              )
        {
            Timestamp = timestamp;
        }

        public static Result<ValueObjects.RemoveManyToManyAssociationInput<Models.ComponentConcretization>, Errors> Validate(
            RemoveComponentConcretizationInput self,
            IReadOnlyList<object> path
            )
        {
            return ValueObjects.RemoveManyToManyAssociationInput<Models.ComponentConcretization>.From(
                    parentId: self.GeneralComponentId,
                    associateId: self.ConcreteComponentId,
                    timestamp: self.Timestamp
                  );
        }
    }
}