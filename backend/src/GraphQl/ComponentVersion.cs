using Guid = System.Guid;
using Models = Icon.Models;
using DateTime = System.DateTime;

namespace Icon.GraphQl
{
    public sealed class ComponentVersion
    {
        public static ComponentVersion FromModel(Models.ComponentVersion componentVersion)
        {
            return new ComponentVersion
            {
                Id = componentVersion.Id,
                ComponentId = componentVersion.ComponentId,
                Timestamp = componentVersion.Timestamp
            };
        }

        public Guid Id { get; set; }
        public Guid ComponentId { get; set; }
        public DateTime Timestamp { get; set; }

        public ComponentVersion()
        {
        }
    }
}