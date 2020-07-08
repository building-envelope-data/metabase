using Infrastructure.ValueObjects;
using IPageInfo = HotChocolate.Types.Relay.IPageInfo;

namespace Metabase.GraphQl
{
    public abstract class Connection
    {
        public Id FromId { get; }
        public IPageInfo PageInfo { get; }
        public Timestamp RequestTimestamp { get; }

        protected Connection(
            Id fromId,
            IPageInfo pageInfo,
            Timestamp requestTimestamp
            )
        {
            FromId = fromId;
            PageInfo = pageInfo;
            RequestTimestamp = requestTimestamp;
        }
    }
}