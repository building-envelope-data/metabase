using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Metabase.Data;

namespace Metabase.Authorization;

public sealed class UserMethodDeveloperAuthorization(
    ApplicationDbContext context,
    UserManager<User> userManager
) : CommonMethodAuthorization(context, userManager)
{
    internal async Task<bool> IsAuthorizedToAdd(
        ClaimsPrincipal claimsPrincipal,
        Guid methodId,
        CancellationToken cancellationToken
    )
    {
        var user = await GetUserAsync(claimsPrincipal);
        return user is not null
               && await IsAtLeastAssistantOfVerifiedMethodManager(
                   user,
                   methodId,
                   cancellationToken
               );
    }

    internal async Task<bool> IsAuthorizedToConfirm(
        ClaimsPrincipal claimsPrincipal,
        Guid userId
    )
    {
        var loggedInUser = await GetUserAsync(claimsPrincipal);
        return loggedInUser is not null
               && IsSame(
                   loggedInUser,
                   userId
               );
    }

    internal async Task<bool> IsAuthorizedToRemove(
        ClaimsPrincipal claimsPrincipal,
        Guid methodId,
        CancellationToken cancellationToken
    )
    {
        var user = await GetUserAsync(claimsPrincipal);
        return user is not null
               && await IsAtLeastAssistantOfVerifiedMethodManager(
                   user,
                   methodId,
                   cancellationToken
               );
    }
}