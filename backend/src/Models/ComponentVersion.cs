using Guid = System.Guid;
using DateTime = System.DateTime;

namespace Icon.Models
{
    public sealed class ComponentVersion
      : Model
    {
        public Guid ComponentId { get; }
        public ComponentInformation Information { get; }

        public ComponentVersion(
            Guid id,
            Guid componentId,
            ComponentInformation information,
            DateTime timestamp
            )
          : base(id, timestamp)
        {
            ComponentId = componentId;
            Information = information;
        }
    }
}