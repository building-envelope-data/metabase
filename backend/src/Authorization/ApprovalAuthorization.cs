using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Metabase.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Metabase.Authorization;

public sealed class ApprovalAuthorization
{
    public static async Task<bool> IsAuthorizedToAddApprovals(
        ClaimsPrincipal claimsPrincipal,
        UserManager<User> userManager,
        ApplicationDbContext context,
        CancellationToken cancellationToken)
    {
        var user = await userManager.GetUserAsync(claimsPrincipal).ConfigureAwait(false);
        return user is not null
               && (await context.InstitutionRepresentatives.AsQueryable()
                    .SingleOrDefaultAsync(
                        x => x.UserId == user.Id && x.DataSigningPermission == Enumerations.DataSigningPermission.ALLOWED,
                        cancellationToken
                    ).ConfigureAwait(false)) is not null;
    }
}