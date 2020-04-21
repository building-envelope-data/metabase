using DateTime = System.DateTime;

namespace Icon.GraphQl
{
    public abstract class Payload
    {
        public DateTime Timestamp { get; }

        public Payload(DateTime timestamp)
        {
            Timestamp = timestamp;
        }
    }
}