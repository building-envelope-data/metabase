using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Metabase.GraphQl
{
    public sealed class RemoveComponentConcretizationInput
      : AddOrRemoveComponentConcretizationInput
    {
        public Timestamp Timestamp { get; }

        public RemoveComponentConcretizationInput(
            Id generalComponentId,
            Id concreteComponentId,
            Timestamp timestamp
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