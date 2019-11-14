using Guid = System.Guid;
using Models = Icon.Models;
using DateTime = System.DateTime;

namespace Icon.GraphQl
{
    public sealed class ComponentVersion
    {
        public static ComponentVersion FromModel(
            Models.ComponentVersion componentVersion,
            DateTime requestTimestamp
            )
        {
            return new ComponentVersion
            {
                Id = componentVersion.Id,
                ComponentId = componentVersion.ComponentId,
                Timestamp = componentVersion.Timestamp,
                RequestTimestamp = requestTimestamp
            };
        }

        public Guid Id { get; set; }
        public Guid ComponentId { get; set; }
        public DateTime Timestamp { get; set; }
        public DateTime RequestTimestamp { get; set; }

        public ComponentVersion()
        {
        }
    }
}