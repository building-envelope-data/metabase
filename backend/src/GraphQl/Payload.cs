using GreenDonut;
using ValueObjects = Icon.ValueObjects;

namespace Icon.GraphQl
{
    public abstract class Payload
    {
        public ValueObjects.Timestamp Timestamp { get; } // TODO? Better name it `requestTimestamp` or `fetchTimestamp` or `loadTimestamp` or `queryTimestamp` or ...

        public Payload(ValueObjects.Timestamp timestamp)
        {
            Timestamp = timestamp;
        }
    }
}