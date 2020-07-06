using System;

namespace Infrastructure.Aggregates
{
    public interface IAggregate
    {
        // For indexing our event streams
        public Guid Id { get; }
    }
}