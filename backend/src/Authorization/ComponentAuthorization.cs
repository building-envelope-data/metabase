using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Metabase.Data;

namespace Metabase.Authorization;

public sealed class ComponentAuthorization(
    ApplicationDbContext context,
    UserManager<User> userManager
) : CommonComponentAuthorization(context, userManager)
{
    internal async Task<bool> IsAuthorizedToCreateComponentForInstitution(
        ClaimsPrincipal claimsPrincipal,
        Guid institutionId,
        CancellationToken cancellationToken
    )
    {
        var user = await GetUserAsync(claimsPrincipal);
        return user is not null &&
               await IsAtLeastAssistantOfVerifiedInstitution(
                   user,
                   institutionId,
                   cancellationToken
               );
    }

    internal async Task<bool> IsAuthorizedToUpdate(
        ClaimsPrincipal claimsPrincipal,
        Guid componentId,
        CancellationToken cancellationToken
    )
    {
        var user = await GetUserAsync(claimsPrincipal);
        return user is not null &&
               await IsAtLeastAssistantOfOneVerifiedManufacturerOfComponent(
                   user,
                   componentId,
                   cancellationToken
               );
    }
}