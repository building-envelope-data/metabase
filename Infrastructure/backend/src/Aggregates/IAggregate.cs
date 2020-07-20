using System;

namespace Infrastructure.Aggregates
{
    public interface IAggregate : IValidatable
    {
        public Guid Id { get; }

        public DateTime Timestamp { get; set; }

        // For protecting the state, i.e. conflict prevention
        public int Version { get; }

        public bool Deleted { get; }

        public bool IsVirgin();
    }
}