using Guid = System.Guid;
using DateTime = System.DateTime;

#nullable enable
namespace Icon.Models
{
    public sealed class ComponentManufacturer
      : Model
    {
        public Guid ComponentId { get; }
        public Guid InstitutionId { get; }

        public ComponentManufacturer(
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