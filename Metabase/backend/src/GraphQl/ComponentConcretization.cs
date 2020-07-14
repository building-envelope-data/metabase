using Infrastructure.GraphQl;
using Infrastructure.ValueObjects;
namespace Metabase.GraphQl
{
    public sealed class ComponentConcretization
      : NodeBase
    {
        public static ComponentConcretization FromModel(
            Models.ComponentConcretization model,
            Timestamp requestTimestamp
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

        public Id GeneralComponentId { get; }
        public Id ConcreteComponentId { get; }

        public ComponentConcretization(
            Id id,
            Id generalComponentId,
            Id concreteComponentId,
            Timestamp timestamp,
            Timestamp requestTimestamp
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