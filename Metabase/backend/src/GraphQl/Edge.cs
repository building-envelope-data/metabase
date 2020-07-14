using Infrastructure.GraphQl;
using Infrastructure.ValueObjects;
namespace Metabase.GraphQl
{
    public abstract class Edge
    {
        public Id NodeId { get; }
        public Timestamp Timestamp { get; }
        public Timestamp RequestTimestamp { get; }

        protected Edge(
            Id nodeId,
            Timestamp timestamp,
            Timestamp requestTimestamp
            )
        {
            NodeId = nodeId;
            Timestamp = timestamp;
            RequestTimestamp = requestTimestamp;
        }
    }
}