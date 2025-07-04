using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Metabase.Data;

namespace Metabase.Authorization;

public sealed class ComponentVariantAuthorization(
    ApplicationDbContext context,
    UserManager<User> userManager
) : CommonComponentAuthorization(context, userManager)
{
    internal async Task<bool> IsAuthorizedToAdd(
        ClaimsPrincipal claimsPrincipal,
        Guid componentId,
        CancellationToken cancellationToken
    )
    {
        var user = await GetUserAsync(claimsPrincipal);
        return user is not null
               &&
               await IsAtLeastAssistantOfOneVerifiedManufacturerOfComponent(
                   user,
                   componentId,
                   cancellationToken
               );
    }

    internal async Task<bool> IsAuthorizedToManage(
        ClaimsPrincipal claimsPrincipal,
        Guid ofComponentId,
        Guid toComponentId,
        CancellationToken cancellationToken
    )
    {
        var user = await GetUserAsync(claimsPrincipal);
        return user is not null
               &&
               await IsAtLeastAssistantOfOneVerifiedManufacturerOfComponent(
                   user,
                   ofComponentId,
                   cancellationToken
               )
               &&
               await IsAtLeastAssistantOfOneVerifiedManufacturerOfComponent(
                   user,
                   toComponentId,
                   cancellationToken
               );
    }
}