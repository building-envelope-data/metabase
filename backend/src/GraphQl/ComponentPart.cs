namespace Icon.GraphQl
{
    public sealed class ComponentPart
      : NodeBase
    {
        public static ComponentPart FromModel(
            Models.ComponentPart model,
            ValueObjects.Timestamp requestTimestamp
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

        public ValueObjects.Id AssembledComponentId { get; }
        public ValueObjects.Id PartComponentId { get; }

        public ComponentPart(
            ValueObjects.Id id,
            ValueObjects.Id assembledComponentId,
            ValueObjects.Id partComponentId,
            ValueObjects.Timestamp timestamp,
            ValueObjects.Timestamp requestTimestamp
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