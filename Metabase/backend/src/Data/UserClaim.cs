using Microsoft.AspNetCore.Identity;
using Guid = System.Guid;

namespace Metabase.Data
{
    public sealed class UserClaim : IdentityUserClaim<Guid>
    {
        public User User { get; set; } = null!;
    }
}