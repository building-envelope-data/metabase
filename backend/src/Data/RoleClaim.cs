using Microsoft.AspNetCore.Identity;
using Guid = System.Guid;

namespace Metabase.Data;

public sealed class RoleClaim : IdentityRoleClaim<Guid>
{
    // public Role Role { get; set; } = default!;
}