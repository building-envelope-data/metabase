using GreenDonut;
using System.Collections.Generic;
using HotChocolate;
using System.Threading.Tasks;
using ValueObjects = Icon.ValueObjects;

namespace Icon.GraphQl
{
    public sealed class AddComponentConcretizationPayload
      : AddOrRemoveComponentConcretizationPayload
    {
        public AddComponentConcretizationPayload(
            ComponentConcretization componentConcretization
            )
          : base(
              generalComponentId: componentConcretization.GeneralComponentId,
              concreteComponentId: componentConcretization.ConcreteComponentId,
              requestTimestamp: componentConcretization.RequestTimestamp
              )
        {
        }
    }
}