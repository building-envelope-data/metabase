using System;

namespace Icon.Infrastructure.Aggregates
{
    public interface IAggregate
    {
        // For indexing our event streams
        public Guid Id { get; }
    }
}