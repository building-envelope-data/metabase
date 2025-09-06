using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Core;
using Metabase.Data;
using Metabase.Data.OpenIdConnect;
using Metabase.Enumerations;

namespace Metabase.Authorization;

public sealed class DataFormatAuthorization(
    IDbContextFactory<ApplicationDbContext> dbContextFactory,
    UserManager<User> userManager,
    OpenIddictApplicationManager<OpenIdConnectApplication> applicationManager
) : CommonAuthorization(dbContextFactory, userManager, applicationManager)
{
    internal Task<bool> IsAuthorizedToCreateDataFormatForInstitution(
        ClaimsPrincipal claimsPrincipal,
        Guid institutionId,
        CancellationToken cancellationToken
    )
    {
        return AuthorizeAsync(
            claimsPrincipal,
            user => IsAtLeastAssistantOfVerifiedInstitution(
                user,
                institutionId,
                cancellationToken
            ),
            application => BelongsToVerifiedInstitution(
                application,
                institutionId,
                cancellationToken
            ),
            cancellationToken
        );
    }

    internal Task<bool> IsAuthorizedToUpdate(
        ClaimsPrincipal claimsPrincipal,
        Guid dataFormatId,
        CancellationToken cancellationToken
    )
    {
        return AuthorizeAsync(
            claimsPrincipal,
            user => IsAtLeastAssistantOfVerifiedDataFormatManager(
                user,
                dataFormatId,
                cancellationToken
            ),
            application => BelongsToVerifiedDataFormatManager(
                application,
                dataFormatId,
                cancellationToken
            ),
            cancellationToken
        );
    }

    private async Task<bool> IsAtLeastAssistantOfVerifiedDataFormatManager(
        User user,
        Guid dataFormatId,
        CancellationToken cancellationToken
    )
    {
        var managerId = await QueryManagerId(dataFormatId, cancellationToken);
        return managerId is not null
            && await IsAtLeastAssistantOfVerifiedInstitution(user, managerId ?? Guid.Empty, cancellationToken);
    }

    private async Task<Guid?> QueryManagerId(Guid dataFormatId, CancellationToken cancellationToken)
    {
        return (
            await Context.DataFormats.AsNoTracking()
                .Where(x => x.Id == dataFormatId)
                .Select(x => new { x.ManagerId })
                .SingleOrDefaultAsync(cancellationToken)
        )?.ManagerId;
    }

    private Task<bool> BelongsToVerifiedDataFormatManager(
        OpenIdConnectApplication application,
        Guid dataFormatId,
        CancellationToken cancellationToken
    )
    {
        return Context.DataFormats.AsNoTracking()
            .Where(f => f.Id == dataFormatId)
            .Where(f => f.Manager != null && f.Manager.State == InstitutionState.VERIFIED)
            .Where(d => d.Manager != null && (
                d.Manager.Id == application.OwnerId
                || d.Manager.ManagerId == application.OwnerId
                || d.Manager.Manager != null && d.Manager.Manager.ManagerId == application.OwnerId
            ))
            .AnyAsync(cancellationToken);
    }
}