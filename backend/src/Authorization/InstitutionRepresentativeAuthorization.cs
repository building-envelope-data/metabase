using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Metabase.Data;

namespace Metabase.Authorization;

public sealed class InstitutionRepresentativeAuthorization(
    ApplicationDbContext context,
    UserManager<User> userManager
) : CommonAuthorization(context, userManager)
{
    internal async Task<bool> IsAuthorizedToManage(
        ClaimsPrincipal claimsPrincipal,
        Guid institutionId,
        CancellationToken cancellationToken
    )
    {
        var user = await GetUserAsync(claimsPrincipal);
        return user is not null
               && await IsOwnerOfVerifiedInstitution(
                   user,
                   institutionId,
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

    internal async Task<bool> IsAuthorizedToManageSigningPermission(
        ClaimsPrincipal claimsPrincipal,
        Guid institutionId,
        CancellationToken cancellationToken
    )
    {
        var user = await GetUserAsync(claimsPrincipal);

        return user is not null
               && (await IsAdministrator(user)
               || await IsOwnerOfInstitution(user, institutionId, cancellationToken));
    }

    internal async Task<bool> IsAuthorizedToAddKeyFingerprint(
        ClaimsPrincipal claimsPrincipal,
        CancellationToken cancellationToken
    )
    {
        var user = await GetUserAsync(claimsPrincipal);
        return user is not null
            && await IsAtLeastAssistant(user, cancellationToken);
    }
}