namespace Icon.GraphQl
{
    public abstract class Edge
    {
        public ValueObjects.Id NodeId { get; }
        public ValueObjects.Timestamp Timestamp { get; }
        public ValueObjects.Timestamp RequestTimestamp { get; }

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