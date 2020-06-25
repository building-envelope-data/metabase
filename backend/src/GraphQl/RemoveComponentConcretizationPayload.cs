namespace Icon.GraphQl
{
    public sealed class RemoveComponentConcretizationPayload
      : AddOrRemoveComponentConcretizationPayload
    {
        public RemoveComponentConcretizationPayload(
            ValueObjects.Id generalComponentId,
            ValueObjects.Id concreteComponentId,
            ValueObjects.Timestamp requestTimestamp
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