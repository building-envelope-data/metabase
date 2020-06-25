namespace Icon.GraphQl
{
    public sealed class ComponentConcretization
      : NodeBase
    {
        public static ComponentConcretization FromModel(
            Models.ComponentConcretization model,
            ValueObjects.Timestamp requestTimestamp
            )
        {
            return new ComponentConcretization(
                id: model.Id,
                generalComponentId: model.GeneralComponentId,
                concreteComponentId: model.ConcreteComponentId,
                timestamp: model.Timestamp,
                requestTimestamp: requestTimestamp
                );
        }

        public ValueObjects.Id GeneralComponentId { get; }
        public ValueObjects.Id ConcreteComponentId { get; }

        public ComponentConcretization(
            ValueObjects.Id id,
            ValueObjects.Id generalComponentId,
            ValueObjects.Id concreteComponentId,
            ValueObjects.Timestamp timestamp,
            ValueObjects.Timestamp requestTimestamp
            )
          : base(
              id: id,
              timestamp: timestamp,
              requestTimestamp: requestTimestamp
              )
        {
            GeneralComponentId = generalComponentId;
            ConcreteComponentId = concreteComponentId;
        }
    }
}