using Infrastructure.GraphQl;
using Infrastructure.ValueObjects;
namespace Metabase.GraphQl
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