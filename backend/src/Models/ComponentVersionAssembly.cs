using Guid = System.Guid;
using DateTime = System.DateTime;

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
          EnsureValid();
        }

        public override bool IsValid()
        {
            return
              base.IsValid() &&
              SuperComponentVersionId != Guid.Empty &&
              SubComponentVersionId != Guid.Empty;
        }
    }
}