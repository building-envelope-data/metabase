using Microsoft.AspNetCore.Identity;
using Guid = System.Guid;

namespace Metabase.Data
{
    public sealed class UserLogin : IdentityUserLogin<Guid>
    {
        public User User { get; set; } = default!;
    }
}