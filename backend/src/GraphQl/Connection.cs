using IPageInfo = HotChocolate.Types.Relay.IPageInfo;

namespace Icon.GraphQl
{
    public abstract class Connection
    {
        public ValueObjects.Id FromId { get; }
        public IPageInfo PageInfo { get; }
        public ValueObjects.Timestamp RequestTimestamp { get; }

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