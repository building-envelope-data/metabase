using Guid = System.Guid;
using Models = Icon.Models;
using DateTime = System.DateTime;

namespace Icon.GraphQl
{
    public abstract class Node
      : INode
    {
        public Guid Id { get; }
        public DateTime Timestamp { get; }
        public DateTime RequestTimestamp { get; } // TODO? Better name it `fetchTimestamp` or `loadTimestamp` or `queryTimestamp` or ...

        public Node(
            Guid id,
            DateTime timestamp,
            DateTime requestTimestamp
            )
        {
            Id = id;
            Timestamp = timestamp;
            RequestTimestamp = requestTimestamp;
        }
    }
}