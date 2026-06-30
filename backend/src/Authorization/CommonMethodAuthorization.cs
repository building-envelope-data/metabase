using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Metabase.Data;
using Metabase.Data.OpenIdConnect;
using Metabase.Enumerations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Core;

namespace Metabase.Authorization;

public abstract class CommonMethodAuthorization(
    IDbContextFactory<ApplicationDbContext> dbContextFactory,
    UserManager<User> userManager,
    OpenIddictApplicationManager<OpenIdConnectApplication> applicationManager
) : CommonAuthorization(dbContextFactory, userManager, applicationManager)
{
    protected async Task<bool> IsAtLeastAssistantOfVerifiedMethodManager(
        User user,
        Guid methodId,
        CancellationToken cancellationToken
    )
    {
        var wrappedManagerId =
            await Context.Methods.AsNoTracking()
                .Where(_ => _.Id == methodId)
                .Select(x => new { x.ManagerId })
                .SingleOrDefaultAsync(cancellationToken);
        if (wrappedManagerId is null)
        {
            return false;
        }
        return await IsAtLeastAssistantOfVerifiedInstitution(
            user, wrappedManagerId.ManagerId, cancellationToken
        );
    }

    protected Task<bool> BelongsToVerifiedMethodManager(
        OpenIdConnectApplication application,
        Guid methodId,
        CancellationToken cancellationToken
    )
    {
        return Context.Methods.AsNoTracking()
            .Where(method => method.Id == methodId)
            .Where(method => method.Manager != null && method.Manager.State == InstitutionState.VERIFIED)
            .Where(method => method.Manager != null && (
                method.Manager.Id == application.OwnerId
                || method.Manager.ManagerId == application.OwnerId
                || method.Manager.Manager != null && method.Manager.Manager.ManagerId == application.OwnerId
            ))
            .AnyAsync(cancellationToken);
    }
}
