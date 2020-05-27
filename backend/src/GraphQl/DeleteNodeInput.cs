using ValueObjects = Icon.ValueObjects;

namespace Icon.GraphQl
{
    public abstract class DeleteNodeInput
    {
        public ValueObjects.Id Id { get; }
        public ValueObjects.Timestamp Timestamp { get; }

        public DeleteNodeInput(
            ValueObjects.Id id,
            ValueObjects.Timestamp timestamp
            )
        {
            Id = id;
            Timestamp = timestamp;
        }
    }
}