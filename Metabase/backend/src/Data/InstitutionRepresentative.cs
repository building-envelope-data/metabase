using System;
using System.ComponentModel.DataAnnotations;

namespace Metabase.Data
{
    public sealed class InstitutionRepresentative
    {
        public Guid InstitutionId { get; set; }
        public Institution Institution { get; set; } = default!;

        public Guid UserId { get; set; }
        public User User { get; set; } = default!;

        [Required]
        public Enumerations.InstitutionRepresentativeRole Role { get; set; }
    }
}