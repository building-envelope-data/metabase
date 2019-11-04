using Guid = System.Guid;
using Icon.Infrastructure.Query;
/* using ZonedDateTime = NodaTime.ZonedDateTime; */
using DateTime = System.DateTime;
using Models = Icon.Models;

namespace Icon.Queries
{
    public class GetComponent : IQuery<Models.Component>
    {
        public Guid ComponentId { get; }
        public DateTime? Timestamp { get; } // TODO ZonedDateTime

        public GetComponent(
            Guid componentId,
            DateTime? timestamp
            )
        {
            ComponentId = componentId;
            Timestamp = timestamp;
        }
    }
}