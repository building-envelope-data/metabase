using Guid = System.Guid;
using DateTime = System.DateTime;

namespace Icon.Models
{
    public sealed class InstitutionRepresentative
      : Model
    {
        public Guid InstitutionId { get; }
        public Guid UserId { get; }
        public InstitutionRepresentativeRole Role { get; }

        public InstitutionRepresentative(
            Guid id,
            Guid institutionId,
            Guid userId,
            InstitutionRepresentativeRole role,
            DateTime timestamp
            )
          : base(id, timestamp)
        {
            InstitutionId = institutionId;
            UserId = userId;
            Role = role;
            EnsureValid();
        }

        public override bool IsValid()
        {
            return
              base.IsValid() &&
              InstitutionId != Guid.Empty &&
              UserId != Guid.Empty;
        }
    }
}