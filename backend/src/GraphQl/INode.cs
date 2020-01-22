using Guid = System.Guid;
using DateTime = System.DateTime;

namespace Icon.GraphQl
{
    public interface INode
    {
        public Guid Id { get; }
        public DateTime Timestamp { get; }
        public DateTime RequestTimestamp { get; }
    }
}