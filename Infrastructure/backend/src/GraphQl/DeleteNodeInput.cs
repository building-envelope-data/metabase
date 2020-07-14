using Infrastructure.GraphQl;
using Infrastructure.ValueObjects;

namespace Infrastructure.GraphQl
{
    public abstract class DeleteNodeInput
    {
        public Id Id { get; }
        public Timestamp Timestamp { get; }

        protected DeleteNodeInput(
            Id id,
            Timestamp timestamp
            )
        {
            Id = id;
            Timestamp = timestamp;
        }
    }
}