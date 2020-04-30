using IPageInfo = HotChocolate.Types.Relay.IPageInfo;

namespace Icon.GraphQl
{
    public abstract class Connection
    {
        public ValueObjects.Id FromId { get; }
        public IPageInfo PageInfo { get; }
        public ValueObjects.Timestamp RequestTimestamp { get; } // TODO? Better name it `timestamp` or `fetchTimestamp` or `loadTimestamp` or `queryTimestamp` or ...

        public Connection(
            ValueObjects.Id fromId,
            IPageInfo pageInfo,
            ValueObjects.Timestamp requestTimestamp
            )
        {
            FromId = fromId;
            PageInfo = pageInfo;
            RequestTimestamp = requestTimestamp;
        }
    }
}