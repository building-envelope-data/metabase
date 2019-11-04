using Guid = System.Guid;
using Icon.Infrastructure.Query;
/* using ZonedDateTime = NodaTime.ZonedDateTime; */
using DateTime = System.DateTime;
using Models = Icon.Models;

namespace Icon.Queries
{
    public class GetComponent : IQuery<Models.Component>
    {
        public Guid ComponentId { get; set; }
        public DateTime Timestamp { get; set; } // TODO ZonedDateTime
    }
}
