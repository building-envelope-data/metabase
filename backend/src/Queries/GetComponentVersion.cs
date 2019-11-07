using Guid = System.Guid;
using Icon.Infrastructure.Query;
/* using ZonedDateTime = NodaTime.ZonedDateTime; */
using DateTime = System.DateTime;
using Models = Icon.Models;

namespace Icon.Queries
{
    public class GetComponentVersion : IQuery<Models.ComponentVersion>
    {
        public Guid ComponentVersionId { get; }
        public DateTime Timestamp { get; } // TODO ZonedDateTime

        public GetComponentVersion(
            Guid componentVersionId,
            DateTime timestamp
            )
        {
            ComponentVersionId = componentVersionId;
            Timestamp = timestamp;
        }
    }
}