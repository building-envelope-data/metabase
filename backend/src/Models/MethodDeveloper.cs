using Guid = System.Guid;
using DateTime = System.DateTime;

namespace Icon.Models
{
    public sealed class MethodDeveloper
      : Model
    {
        public Guid MethodId { get; }
        public Guid StakeholderId { get; }

        public MethodDeveloper(
            Guid id,
            Guid methodId,
            Guid stakeholderId,
            DateTime timestamp
            )
          : base(id, timestamp)
        {
            MethodId = methodId;
            StakeholderId = stakeholderId;
            EnsureValid();
        }

        public override bool IsValid()
        {
            return
              base.IsValid() &&
              MethodId != Guid.Empty &&
              StakeholderId != Guid.Empty;
        }
    }
}