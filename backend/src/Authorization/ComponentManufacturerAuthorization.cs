using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using OpenIddict.Core;
using Metabase.Data;
using Metabase.Data.OpenIdConnect;
using Microsoft.EntityFrameworkCore;

namespace Metabase.Authorization;

public sealed class ComponentManufacturerAuthorization(
    IDbContextFactory<ApplicationDbContext> dbContextFactory,
    UserManager<User> userManager,
    OpenIddictApplicationManager<OpenIdConnectApplication> applicationManager
) : CommonComponentAuthorization(dbContextFactory, userManager, applicationManager)
{
    internal Task<bool> IsAuthorizedToAdd(
        ClaimsPrincipal claimsPrincipal,
        Guid componentId,
        CancellationToken cancellationToken
    )
    {
        return AuthorizeAsync(
            claimsPrincipal,
            user => IsAtLeastAssistantOfOneVerifiedManufacturerOfComponent(
                user,
                componentId,
                cancellationToken
            ),
            application => BelongsToVerifiedManufacturerOfComponent(
                application,
                componentId,
                cancellationToken
            ),
            cancellationToken
        );
    }

    internal Task<bool> IsAuthorizedToConfirm(
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

    internal Task<bool> IsAuthorizedToRemove(
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
}