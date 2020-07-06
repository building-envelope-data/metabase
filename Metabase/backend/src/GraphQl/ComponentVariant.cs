using Infrastructure.ValueObjects;
namespace Metabase.GraphQl
{
    public sealed class ComponentVariant
      : NodeBase
    {
        public static ComponentVariant FromModel(
            Models.ComponentVariant model,
            Timestamp requestTimestamp
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

        public Id BaseComponentId { get; }
        public Id VariantComponentId { get; }

        public ComponentVariant(
            Id id,
            Id baseComponentId,
            Id variantComponentId,
            Timestamp timestamp,
            Timestamp requestTimestamp
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