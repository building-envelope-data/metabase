using Guid = System.Guid;
using DateTime = System.DateTime;

namespace Icon.Models
{
    public sealed class ComponentVersion
    {
        public Guid Id { get; }
        public Guid ComponentId { get; }
        public DateTime Timestamp { get; }

        public ComponentVersion(Guid id, Guid componentId, DateTime timestamp)
        {
            Id = id;
            ComponentId = componentId;
            Timestamp = timestamp;
        }
    }
}