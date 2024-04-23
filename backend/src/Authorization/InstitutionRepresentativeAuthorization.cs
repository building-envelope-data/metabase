using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Metabase.Authorization;

public static class InstitutionRepresentativeAuthorization
{
    public static async Task<bool> IsAuthorizedToManage(
        ClaimsPrincipal claimsPrincipal,
        Guid institutionId,
        UserManager<Data.User> userManager,
        Data.ApplicationDbContext context,
        CancellationToken cancellationToken
    )
    {
        var user = await userManager.GetUserAsync(claimsPrincipal).ConfigureAwait(false);
        return user is not null
               && await CommonAuthorization.IsOwnerOfVerifiedInstitution(
                   user,
                   institutionId,
                   context,
                   cancellationToken
               );
    }

    public static async Task<bool> IsAuthorizedToConfirm(
        ClaimsPrincipal claimsPrincipal,
        Guid userId,
        UserManager<Data.User> userManager
    )
    {
        var loggedInUser = await userManager.GetUserAsync(claimsPrincipal).ConfigureAwait(false);
        return loggedInUser is not null
               && CommonAuthorization.IsSame(
                   loggedInUser,
                   userId
               );
    }
}