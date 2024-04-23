using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Metabase.Data;
using Microsoft.AspNetCore.Identity;

namespace Metabase.Authorization;

public static class ComponentManufacturerAuthorization
{
    public static async Task<bool> IsAuthorizedToAdd(
        ClaimsPrincipal claimsPrincipal,
        Guid componentId,
        UserManager<User> userManager,
        ApplicationDbContext context,
        CancellationToken cancellationToken
    )
    {
        var user = await userManager.GetUserAsync(claimsPrincipal).ConfigureAwait(false);
        return user is not null
               &&
               await CommonComponentAuthorization.IsAtLeastAssistantOfOneVerifiedManufacturerOfComponent(
                   user,
                   componentId,
                   context,
                   cancellationToken
               );
    }

    public static async Task<bool> IsAuthorizedToConfirm(
        ClaimsPrincipal claimsPrincipal,
        Guid institutionId,
        UserManager<User> userManager,
        ApplicationDbContext context,
        CancellationToken cancellationToken
    )
    {
        var user = await userManager.GetUserAsync(claimsPrincipal).ConfigureAwait(false);
        return user is not null
               &&
               await CommonAuthorization.IsAtLeastAssistantOfVerifiedInstitution(
                   user,
                   institutionId,
                   context,
                   cancellationToken
               );
    }

    public static async Task<bool> IsAuthorizedToRemove(
        ClaimsPrincipal claimsPrincipal,
        Guid institutionId,
        UserManager<User> userManager,
        ApplicationDbContext context,
        CancellationToken cancellationToken
    )
    {
        var user = await userManager.GetUserAsync(claimsPrincipal).ConfigureAwait(false);
        return user is not null
               &&
               await CommonAuthorization.IsAtLeastAssistantOfVerifiedInstitution(
                   user,
                   institutionId,
                   context,
                   cancellationToken
               );
    }
}