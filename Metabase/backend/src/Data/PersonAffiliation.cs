using System;

namespace Metabase.Data
{
    public sealed class PersonAffiliation
      : Infrastructure.Data.Entity
    {
        public Guid PersonId { get; set; }
        public Person Person { get; set; } = default!;

        public Guid InstitutionId { get; set; }
        public Institution Institution { get; set; } = default!;
    }
}