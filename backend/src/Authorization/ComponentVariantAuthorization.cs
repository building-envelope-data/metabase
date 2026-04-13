using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Metabase.Data;
using Metabase.Data.OpenIdConnect;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Core;

namespace Metabase.Authorization;

public sealed class ComponentVariantAuthorization(
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
            user => IsAtLeastAssistantOfVerifiedComponentManager(
                user,
                componentId,
                cancellationToken
            ),
            application => BelongsToVerifiedComponentManager(
                application,
                componentId,
                cancellationToken
            ),
            cancellationToken
        );
    }

    internal Task<bool> IsAuthorizedToManage(
        ClaimsPrincipal claimsPrincipal,
        Guid ofComponentId,
        Guid toComponentId,
        CancellationToken cancellationToken
    )
    {
        return AuthorizeAsync(
            claimsPrincipal,
            async user => await IsAtLeastAssistantOfVerifiedComponentManager(
                user,
                ofComponentId,
                cancellationToken
            )
            && await IsAtLeastAssistantOfVerifiedComponentManager(
                user,
                toComponentId,
                cancellationToken
            ),
            async application => await BelongsToVerifiedComponentManager(
                application,
                ofComponentId,
                cancellationToken
            )
            && await BelongsToVerifiedComponentManager(
                application,
                toComponentId,
                cancellationToken
            ),
            cancellationToken
        );
    }
}