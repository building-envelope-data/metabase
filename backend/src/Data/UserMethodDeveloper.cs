using System;

namespace Metabase.Data
{
    public sealed class UserMethodDeveloper
      : Data.Entity,
        IMethodDeveloper
    {
        public Guid MethodId { get; set; }
        public Method Method { get; set; } = default!;

        public Guid UserId { get; set; }
        public User User { get; set; } = default!;
    }
}