using Infrastructure.ValueObjects;
namespace Metabase.GraphQl
{
    public abstract class Payload
    {
        public Timestamp RequestTimestamp { get; } // Alternative verbs: fetch, load, query

        public Payload(Timestamp requestTimestamp)
        {
            RequestTimestamp = requestTimestamp;
        }
    }
}