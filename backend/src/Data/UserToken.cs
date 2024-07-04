using Microsoft.AspNetCore.Identity;
using Guid = System.Guid;

namespace Metabase.Data;

public sealed class UserToken : IdentityUserToken<Guid>
{
    // public User User { get; set; } = default!;
}