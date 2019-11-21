using Guid = System.Guid;
using DateTime = System.DateTime;

namespace Icon.Models
{
    public sealed class ComponentAssembly
      : Model
    {
        public Guid SuperComponentId { get; }
        public Guid SubComponentId { get; }

        public ComponentAssembly(
            Guid id,
            Guid superComponentId,
            Guid subComponentId,
            DateTime timestamp
            )
          : base(id, timestamp)
        {
            SuperComponentId = superComponentId;
            SubComponentId = subComponentId;
          EnsureValid();
        }

        public override bool IsValid()
        {
            return
              base.IsValid() &&
              SuperComponentId != Guid.Empty &&
              SubComponentId != Guid.Empty;
        }
    }
}