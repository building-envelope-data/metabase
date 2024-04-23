using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Metabase.Data;
using Microsoft.AspNetCore.Identity;

namespace Metabase.Authorization;

public static class ComponentGeneralizationAuthorization
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

    public static async Task<bool> IsAuthorizedToManage(
        ClaimsPrincipal claimsPrincipal,
        Guid assembledComponentId,
        Guid partComponentId,
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
                   assembledComponentId,
                   context,
                   cancellationToken
               )
               &&
               await CommonComponentAuthorization.IsAtLeastAssistantOfOneVerifiedManufacturerOfComponent(
                   user,
                   partComponentId,
                   context,
                   cancellationToken
               );
    }
}