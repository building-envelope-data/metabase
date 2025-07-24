using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Core;
using Metabase.Data;
using Metabase.Data.OpenIdConnect;
using Metabase.Enumerations;

namespace Metabase.Authorization;

public abstract class CommonMethodAuthorization(
    ApplicationDbContext context,
    UserManager<User> userManager,
    OpenIddictApplicationManager<OpenIdConnectApplication> applicationManager
) : CommonAuthorization(context, userManager, applicationManager)
{
    protected async Task<bool> IsAtLeastAssistantOfVerifiedMethodManager(
        User user,
        Guid methodId,
        CancellationToken cancellationToken
    )
    {
        var wrappedManagerId =
            await Context.Methods.AsNoTracking()
                .Where(x => x.Id == methodId)
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
                method.Manager.OpenIdConnectApplicationEdges.Any(e => e.ApplicationId == application.Id)
                || method.Manager.Manager != null && method.Manager.Manager.OpenIdConnectApplicationEdges.Any(e => e.ApplicationId == application.Id)
                || method.Manager.Manager != null && method.Manager.Manager.Manager != null && method.Manager.Manager.Manager.OpenIdConnectApplicationEdges.Any(e => e.ApplicationId == application.Id)
            ))
            .AnyAsync(cancellationToken);
    }
}