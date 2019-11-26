using ValueObjects = Icon.ValueObjects;
using DateTime = System.DateTime;

namespace Icon.Models
{
    public abstract class Model
    {
        public ValueObjects.Id Id { get; }
        public ValueObjects.Timestamp Timestamp { get; }

        public Model(
            ValueObjects.Id id,
            ValueObjects.Timestamp timestamp
            )
        {
            Id = id;
            Timestamp = timestamp;
        }
    }
}