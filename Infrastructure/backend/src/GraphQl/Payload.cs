using Infrastructure.ValueObjects;

namespace Infrastructure.GraphQl
{
    public abstract class Payload
    {
        public Timestamp RequestTimestamp { get; } // Alternative verbs: fetch, load, query

        protected Payload(Timestamp requestTimestamp)
        {
            RequestTimestamp = requestTimestamp;
        }
    }
}