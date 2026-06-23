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

public abstract class CommonComponentAuthorization(
    IDbContextFactory<ApplicationDbContext> dbContextFactory,
    UserManager<User> userManager,
    OpenIddictApplicationManager<OpenIdConnectApplication> applicationManager
) : CommonAuthorization(dbContextFactory, userManager, applicationManager)
{
    protected async Task<bool> IsAtLeastAssistantOfVerifiedComponentManager(
        User user,
        Guid componentId,
        CancellationToken cancellationToken
    )
    {
        var wrappedManagerId =
            await Context.Components.AsNoTracking()
                .Where(_ => _.Id == componentId)
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

    protected Task<bool> BelongsToVerifiedComponentManager(
        OpenIdConnectApplication application,
        Guid componentId,
        CancellationToken cancellationToken
    )
    {
        return Context.Components.AsNoTracking()
            .Where(component => component.Id == componentId)
            .Where(component => component.Manager != null && component.Manager.State == InstitutionState.VERIFIED)
            .Where(component => component.Manager != null && (
                component.Manager.Id == application.OwnerId
                || component.Manager.ManagerId == application.OwnerId
                || component.Manager.Manager != null && component.Manager.Manager.ManagerId == application.OwnerId
            ))
            .AnyAsync(cancellationToken);
    }
}