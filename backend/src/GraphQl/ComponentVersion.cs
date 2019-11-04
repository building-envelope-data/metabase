using Guid = System.Guid;
using Models = Icon.Models;

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
                Version = componentVersion.Version
            };
        }

        public Guid Id { get; set; }
        public Guid ComponentId { get; set; }
        public int Version { get; set; }

        public ComponentVersion()
        {
        }
    }
}