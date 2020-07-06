using Infrastructure.ValueObjects;
namespace Metabase.GraphQl
{
    public sealed class ComponentPart
      : NodeBase
    {
        public static ComponentPart FromModel(
            Models.ComponentPart model,
            Timestamp requestTimestamp
            )
        {
            return new ComponentPart(
                id: model.Id,
                assembledComponentId: model.AssembledComponentId,
                partComponentId: model.PartComponentId,
                timestamp: model.Timestamp,
                requestTimestamp: requestTimestamp
                );
        }

        public Id AssembledComponentId { get; }
        public Id PartComponentId { get; }

        public ComponentPart(
            Id id,
            Id assembledComponentId,
            Id partComponentId,
            Timestamp timestamp,
            Timestamp requestTimestamp
            )
          : base(
              id: id,
              timestamp: timestamp,
              requestTimestamp: requestTimestamp
              )
        {
            AssembledComponentId = assembledComponentId;
            PartComponentId = partComponentId;
        }
    }
}