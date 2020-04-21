using Guid = System.Guid;
using DateTime = System.DateTime;

namespace Icon.GraphQl
{
#pragma warning disable SA1302
    public interface Node
#pragma warning restore SA1302
    {
        public Guid Id { get; }
        public DateTime Timestamp { get; }
        public DateTime RequestTimestamp { get; }
    }
}