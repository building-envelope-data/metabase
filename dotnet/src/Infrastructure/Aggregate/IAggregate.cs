using System;

namespace Icon.Infrastructure.Aggregate
{
    public interface IAggregate
    {
        Guid Id { get; }
    }
}