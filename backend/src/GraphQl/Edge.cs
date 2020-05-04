namespace Icon.GraphQl
{
    public abstract class Edge
    {
        public ValueObjects.Id NodeId { get; }
        public ValueObjects.Timestamp Timestamp { get; } // TODO? Better name it `edgeTimestamp` or `fetchTimestamp` or `loadTimestamp` or `queryTimestamp` or ...
        public ValueObjects.Timestamp RequestTimestamp { get; } // TODO? Better name it `timestamp` or `fetchTimestamp` or `loadTimestamp` or `queryTimestamp` or ...

        public Edge(
            ValueObjects.Id nodeId,
            ValueObjects.Timestamp timestamp,
            ValueObjects.Timestamp requestTimestamp
            )
        {
            NodeId = nodeId;
            Timestamp = timestamp;
            RequestTimestamp = requestTimestamp;
        }
    }
}