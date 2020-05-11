using GreenDonut;
using ValueObjects = Icon.ValueObjects;

namespace Icon.GraphQl
{
    public abstract class Payload
    {
        public ValueObjects.Timestamp RequestTimestamp { get; } // Alternative verbs: fetch, load, query

        public Payload(ValueObjects.Timestamp requestTimestamp)
        {
            RequestTimestamp = requestTimestamp;
        }
    }
}