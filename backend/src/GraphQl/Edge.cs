namespace Icon.GraphQl
{
    public abstract class Edge
    {
        public ValueObjects.Id NodeId { get; }
        public ValueObjects.Timestamp RequestTimestamp { get; } // TODO? Better name it `timestamp` or `fetchTimestamp` or `loadTimestamp` or `queryTimestamp` or ...

        public Edge(
            ValueObjects.Id nodeId,
            ValueObjects.Timestamp requestTimestamp
            )
        {
            NodeId = nodeId;
            RequestTimestamp = requestTimestamp;
        }
    }
}