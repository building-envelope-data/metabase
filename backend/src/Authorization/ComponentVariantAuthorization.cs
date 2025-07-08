using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using OpenIddict.Core;
using Metabase.Data;
using Metabase.Data.OpenIdConnect;

namespace Metabase.Authorization;

public sealed class ComponentVariantAuthorization(
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
            application => BelongsToVerifiedManufacturerOfComponent(
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
            async user => await IsAtLeastAssistantOfOneVerifiedManufacturerOfComponent(
                user,
                ofComponentId,
                cancellationToken
            )
            && await IsAtLeastAssistantOfOneVerifiedManufacturerOfComponent(
                user,
                toComponentId,
                cancellationToken
            ),
            async application => await BelongsToVerifiedManufacturerOfComponent(
                application,
                ofComponentId,
                cancellationToken
            )
            && await BelongsToVerifiedManufacturerOfComponent(
                application,
                toComponentId,
                cancellationToken
            ),
            cancellationToken
        );
    }
}