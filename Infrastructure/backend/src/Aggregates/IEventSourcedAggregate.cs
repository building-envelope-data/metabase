using System;
using Infrastructure.ValueObjects;
using IValidatable = Infrastructure.IValidatable;

namespace Infrastructure.Aggregates
{
    public interface IEventSourcedAggregate : IAggregate, IValidatable
    {
        public DateTime Timestamp { get; set; }

        // For protecting the state, i.e. conflict prevention
        public int Version { get; }

        public bool Deleted { get; }

        public bool IsVirgin();
    }
}