using Guid = System.Guid;
using DateTime = System.DateTime;

namespace Icon.Models
{
    public sealed class Component
    {
        public Guid Id { get; }
        public DateTime Timestamp { get; }

        public Component(Guid id, DateTime timestamp)
        {
            Id = id;
            Timestamp = timestamp;
        }
    }
}