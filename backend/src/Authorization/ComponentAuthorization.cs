using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using OpenIddict.Core;
using Metabase.Data;
using Metabase.Data.OpenIdConnect;

namespace Metabase.Authorization;

public sealed class ComponentAuthorization(
    ApplicationDbContext context,
    UserManager<User> userManager,
    OpenIddictApplicationManager<OpenIdConnectApplication> applicationManager
) : CommonComponentAuthorization(context, userManager, applicationManager)
{
    internal Task<bool> IsAuthorizedToCreateComponentForInstitution(
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
}