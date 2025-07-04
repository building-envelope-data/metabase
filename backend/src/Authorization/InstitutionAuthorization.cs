using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Metabase.Data;

namespace Metabase.Authorization;

public sealed class InstitutionAuthorization(
    ApplicationDbContext context,
    UserManager<User> userManager
) : CommonAuthorization(context, userManager)
{
    internal async Task<bool> IsAuthorizedToUpdateInstitution(
        ClaimsPrincipal claimsPrincipal,
        Guid institutionId,
        CancellationToken cancellationToken
    )
    {
        var user = await GetUserAsync(claimsPrincipal);
        return user is not null
               && await IsAtLeastAssistant(
                   user,
                   institutionId,
                   cancellationToken
               );
    }

    internal async Task<bool> IsAuthorizedToDeleteInstitution(
        ClaimsPrincipal claimsPrincipal,
        Guid institutionId,
        CancellationToken cancellationToken
    )
    {
        var user = await GetUserAsync(claimsPrincipal);
        return user is not null
               && await IsOwnerOfInstitution(
                   user,
                   institutionId,
                   cancellationToken
               );
    }

    internal async Task<bool> IsAuthorizedToCreateInstitutionManagedByInstitution(
        ClaimsPrincipal claimsPrincipal,
        Guid institutionId,
        CancellationToken cancellationToken
    )
    {
        var user = await GetUserAsync(claimsPrincipal);
        return user is not null
               && await IsAtLeastAssistantOfVerifiedInstitution(
                   user,
                   institutionId,
                   cancellationToken
               );
    }

    internal async Task<bool> IsAuthorizedToVerifyInstitution(
        ClaimsPrincipal claimsPrincipal
    )
    {
        var user = await GetUserAsync(claimsPrincipal);
        return user is not null
               && await IsVerifier(user);
    }

    internal async Task<bool> IsAuthorizedToSwitchInstitutionOperatingState(
        ClaimsPrincipal claimsPrincipal,
        Guid institutionId,
        CancellationToken cancellationToken
    )
    {
        var user = await GetUserAsync(claimsPrincipal);
        return user is not null
               && await IsOwnerOfInstitution(
                   user,
                   institutionId,
                   cancellationToken
               );
    }
}