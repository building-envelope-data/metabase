namespace Infrastructure.GraphQl
{
    public abstract class NodeBase
      : Node
    {
        protected static ValueObjects.TimestampedId TimestampId(
            ValueObjects.Id id,
            ValueObjects.Timestamp timestamp
            )
        {
            return ResultHelpers.HandleFailure(
                ValueObjects.TimestampedId.From(
                  id, timestamp
                  )
                );
        }

        public ValueObjects.Id Id { get; }
        public ValueObjects.Timestamp Timestamp { get; }
        public ValueObjects.Timestamp RequestTimestamp { get; }

        protected NodeBase(
            ValueObjects.Id id,
            ValueObjects.Timestamp timestamp,
            ValueObjects.Timestamp requestTimestamp
            )
        {
            Id = id;
            Timestamp = timestamp;
            RequestTimestamp = requestTimestamp;
        }
    }
}