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

public sealed class ComponentGeneralizationAuthorization(
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
        Guid assembledComponentId,
        Guid partComponentId,
        CancellationToken cancellationToken
    )
    {
        return AuthorizeAsync(
            claimsPrincipal,
            async user => await IsAtLeastAssistantOfVerifiedComponentManager(
                user,
                assembledComponentId,
                cancellationToken
            )
            && await IsAtLeastAssistantOfVerifiedComponentManager(
                user,
                partComponentId,
                cancellationToken
            ),
            async application => await BelongsToVerifiedComponentManager(
                application,
                assembledComponentId,
                cancellationToken
            )
            && await BelongsToVerifiedComponentManager(
                application,
                partComponentId,
                cancellationToken
            ),
            cancellationToken
        );
    }
}