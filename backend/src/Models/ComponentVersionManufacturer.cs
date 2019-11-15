using Guid = System.Guid;
using DateTime = System.DateTime;

#nullable enable
namespace Icon.Models
{
    public sealed class ComponentVersionManufacturer
      : Model
    {
        public Guid ComponentId { get; }
        public Guid InstitutionId { get; }

        public ComponentVersionManufacturer(
            Guid id,
            Guid componentId,
            Guid institutionId,
            DateTime timestamp
            )
          : base(id, timestamp)
        {
            ComponentId = componentId;
            InstitutionId = institutionId;
        }
    }
}