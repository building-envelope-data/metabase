using Infrastructure.ValueObjects;
namespace Metabase.GraphQl
{
    public sealed class RemoveComponentConcretizationPayload
      : AddOrRemoveComponentConcretizationPayload
    {
        public RemoveComponentConcretizationPayload(
            Id generalComponentId,
            Id concreteComponentId,
            Timestamp requestTimestamp
            )
          : base(
              generalComponentId: generalComponentId,
              concreteComponentId: concreteComponentId,
              requestTimestamp: requestTimestamp
              )
        {
        }
    }
}