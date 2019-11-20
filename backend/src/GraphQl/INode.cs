using Guid = System.Guid;
using DateTime = System.DateTime;

namespace Icon.GraphQl
{
    public interface INode
    {
        public Guid? Id { get; set; }
        public DateTime? Timestamp { get; set; }
        public DateTime? RequestTimestamp { get; set; }
    }
}