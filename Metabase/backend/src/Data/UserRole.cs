using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Guid = System.Guid;

namespace Metabase.Data
{
    public sealed class UserRole : IdentityUserRole<Guid>
    {
        public User User { get; set; } = null!;
        public Role Role { get; set; } = null!;
    }
}