using DateTime = System.DateTime;
using Models = Icon.Models;

namespace Icon.GraphQl
{
    public sealed class ComponentVariant
      : NodeBase
    {
        public static ComponentVariant FromModel(
            Models.ComponentVariant model,
            ValueObjects.Timestamp requestTimestamp
            )
        {
            return new ComponentVariant(
                id: model.Id,
                baseComponentId: model.BaseComponentId,
                variantComponentId: model.VariantComponentId,
                timestamp: model.Timestamp,
                requestTimestamp: requestTimestamp
                );
        }

        public ValueObjects.Id BaseComponentId { get; }
        public ValueObjects.Id VariantComponentId { get; }

        public ComponentVariant(
            ValueObjects.Id id,
            ValueObjects.Id baseComponentId,
            ValueObjects.Id variantComponentId,
            ValueObjects.Timestamp timestamp,
            ValueObjects.Timestamp requestTimestamp
            )
          : base(
              id: id,
              timestamp: timestamp,
              requestTimestamp: requestTimestamp
              )
        {
            BaseComponentId = baseComponentId;
            VariantComponentId = variantComponentId;
        }
    }
}