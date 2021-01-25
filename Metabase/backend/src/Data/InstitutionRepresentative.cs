using System;

namespace Metabase.Data
{
    public sealed class InstitutionRepresentative
    {
        public Guid InstitutionId { get; set; }
        public Institution Institution { get; set; } = default!;

        public Guid UserId { get; set; }
        public User User { get; set; } = default!;

        public Enumerations.InstitutionRepresentativeRole Role { get; set; }
    }
}