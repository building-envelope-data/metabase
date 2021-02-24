using System;

namespace Metabase.Data
{
    public sealed class PersonMethodDeveloper
      : Infrastructure.Data.Entity
    {
        public Guid MethodId { get; set; }
        public Method Method { get; set; } = default!;

        public Guid PersonId { get; set; }
        public Person Person { get; set; } = default!;
    }
}