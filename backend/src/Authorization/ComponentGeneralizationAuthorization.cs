using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using OpenIddict.Core;
using Metabase.Data;
using Metabase.Data.OpenIdConnect;

namespace Metabase.Authorization;

public sealed class ComponentGeneralizationAuthorization(
    ApplicationDbContext context,
    UserManager<User> userManager,
    OpenIddictApplicationManager<OpenIdConnectApplication> applicationManager
) : CommonComponentAuthorization(context, userManager, applicationManager)
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
            application => Task.FromResult(false),
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
            async user => await IsAtLeastAssistantOfOneVerifiedManufacturerOfComponent(
                user,
                assembledComponentId,
                cancellationToken
            )
            && await IsAtLeastAssistantOfOneVerifiedManufacturerOfComponent(
                user,
                partComponentId,
                cancellationToken
            ),
            application => Task.FromResult(false),
            cancellationToken
        );
    }
}