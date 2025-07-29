using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Core;
using Metabase.Data;
using Metabase.Data.OpenIdConnect;

namespace Metabase.Authorization;

public sealed class ApprovalAuthorization(
    IDbContextFactory<ApplicationDbContext> dbContextFactory,
    UserManager<User> userManager,
    OpenIddictApplicationManager<OpenIdConnectApplication> applicationManager
) : CommonAuthorization(dbContextFactory, userManager, applicationManager)
{
    internal Task<bool> IsAuthorizedToAddApprovals(
        ClaimsPrincipal claimsPrincipal,
        CancellationToken cancellationToken
    )
    {
        return AuthorizeAsync(
            claimsPrincipal,
            async user => (await Context.InstitutionRepresentatives.AsQueryable()
                    .SingleOrDefaultAsync(
                        x => x.UserId == user.Id && x.DataSigningPermission == Enumerations.DataSigningPermission.ALLOWED,
                        cancellationToken
                    )) is not null,
            application => Task.FromResult(false),
            cancellationToken
        );
    }
}