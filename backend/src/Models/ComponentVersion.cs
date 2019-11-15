using Guid = System.Guid;
using DateTime = System.DateTime;

#nullable enable
namespace Icon.Models
{
    public sealed class ComponentVersion
      : Model
    {
        public Guid ComponentId { get; }
        public Guid InformationId { get; }

        public ComponentVersion(
            Guid id,
            Guid componentId,
            Guid informationId,
            DateTime timestamp
            )
          : base(id, timestamp)
        {
            ComponentId = componentId;
            InformationId = informationId;
        }
    }
}