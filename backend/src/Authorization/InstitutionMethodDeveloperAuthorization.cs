using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Metabase.Data;
using Microsoft.AspNetCore.Identity;

namespace Metabase.Authorization;

public static class InstitutionMethodDeveloperAuthorization
{
    public static async Task<bool> IsAuthorizedToAdd(
        ClaimsPrincipal claimsPrincipal,
        Guid methodId,
        UserManager<User> userManager,
        ApplicationDbContext context,
        CancellationToken cancellationToken
    )
    {
        var user = await userManager.GetUserAsync(claimsPrincipal).ConfigureAwait(false);
        return user is not null
               && await CommonMethodAuthorization.IsAtLeastAssistantOfVerifiedMethodManager(
                   user,
                   methodId,
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
               && await CommonAuthorization.IsAtLeastAssistantOfVerifiedInstitution(
                   user,
                   institutionId,
                   context,
                   cancellationToken
               );
    }

    public static async Task<bool> IsAuthorizedToRemove(
        ClaimsPrincipal claimsPrincipal,
        Guid methodId,
        UserManager<User> userManager,
        ApplicationDbContext context,
        CancellationToken cancellationToken
    )
    {
        var user = await userManager.GetUserAsync(claimsPrincipal).ConfigureAwait(false);
        return user is not null
               && await CommonMethodAuthorization.IsAtLeastAssistantOfVerifiedMethodManager(
                   user,
                   methodId,
                   context,
                   cancellationToken
               );
    }
}