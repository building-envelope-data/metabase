using System;

namespace Icon.Infrastructure.Aggregate
{
    public interface IAggregate
    {
        // For indexing our event streams
        public Guid Id { get; }
    }
}