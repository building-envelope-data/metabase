namespace Icon.GraphQl
{
    public abstract class DataX
      : NodeBase
    {
        public ValueObjects.Id ComponentId { get; }
        public object Data { get; }

        public DataX(
            ValueObjects.Id id,
            ValueObjects.Id componentId,
            object data,
            ValueObjects.Timestamp timestamp,
            ValueObjects.Timestamp requestTimestamp
            )
          : base(
              id: id,
              timestamp: timestamp,
              requestTimestamp: requestTimestamp
              )
        {
            ComponentId = componentId;
            Data = data;
        }
    }
}