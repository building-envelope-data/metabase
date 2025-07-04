using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Metabase.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Metabase.Authorization;

public sealed class ApprovalAuthorization(
    ApplicationDbContext context,
    UserManager<User> userManager
) : CommonAuthorization(context, userManager)
{
    internal async Task<bool> IsAuthorizedToAddApprovals(
        ClaimsPrincipal claimsPrincipal,
        CancellationToken cancellationToken
    )
    {
        var user = await GetUserAsync(claimsPrincipal);
        return user is not null
               && (await Context.InstitutionRepresentatives.AsQueryable()
                    .SingleOrDefaultAsync(
                        x => x.UserId == user.Id && x.DataSigningPermission == Enumerations.DataSigningPermission.ALLOWED,
                        cancellationToken
                    ).ConfigureAwait(false)) is not null;
    }
}