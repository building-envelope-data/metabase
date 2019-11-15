using Guid = System.Guid;
using DateTime = System.DateTime;

#nullable enable
namespace Icon.Models
{
    public sealed class ComponentVersionAssembly
      : Model
    {
        public Guid SuperComponentVersionId { get; }
        public Guid SubComponentVersionId { get; }

        public ComponentVersionAssembly(
            Guid id,
            Guid superComponentVersionId,
            Guid subComponentVersionId,
            DateTime timestamp
            )
          : base(id, timestamp)
        {
            SuperComponentVersionId = superComponentVersionId;
            SubComponentVersionId = subComponentVersionId;
        }
    }
}