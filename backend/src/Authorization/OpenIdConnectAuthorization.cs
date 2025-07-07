using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Core;
using Metabase.Data;
using Metabase.Data.OpenIdConnect;

namespace Metabase.Authorization;

public sealed class OpenIdConnectAuthorization(
    ApplicationDbContext context,
    UserManager<User> userManager
) : CommonAuthorization(context, userManager)
{
    internal async Task<bool> IsAuthorizedToViewApplications(
        ClaimsPrincipal claimsPrincipal,
        CancellationToken cancellationToken
    )
    {
        var user = await GetUserAsync(claimsPrincipal);
        return user is not null
               && (await IsAdministrator(user).ConfigureAwait(false)
               || await IsOwner(user, cancellationToken).ConfigureAwait(false));
    }

    internal async Task<bool> IsAuthorizedToManageApplications(
        ClaimsPrincipal claimsPrincipal,
        CancellationToken cancellationToken)
    {
        var user = await GetUserAsync(claimsPrincipal);
        return user is not null
               && (await IsAdministrator(user).ConfigureAwait(false)
               || await IsOwner(user, cancellationToken).ConfigureAwait(false));
    }

    internal async Task<bool> IsAuthorizedToManageApplication(
        ClaimsPrincipal claimsPrincipal,
        Guid applicationId,
        CancellationToken cancellationToken)
    {
        var user = await GetUserAsync(claimsPrincipal);
        var institutionId = await GetInstitutionIdByApplicationId(applicationId, cancellationToken);
        return user is not null
               && (await IsAdministrator(user).ConfigureAwait(false)
               || institutionId is not null && await IsOwnerOfInstitution(user, institutionId ?? Guid.Empty, cancellationToken).ConfigureAwait(false));
    }

    internal async Task<bool> IsAuthorizedToDeleteAuthorization(
        ClaimsPrincipal claimsPrincipal,
        Guid authorizationId,
        OpenIddictAuthorizationManager<Data.OpenIdConnect.OpenIdConnectAuthorization> authorizationManager,
        CancellationToken cancellationToken)
    {
        var user = await GetUserAsync(claimsPrincipal);
        var authorization = await authorizationManager.FindByIdAsync(authorizationId.ToString(), cancellationToken);
        Guid? institutionId = authorization is not null && authorization.Application is not null ? await GetInstitutionIdByApplicationId(authorization.Application.Id, cancellationToken).ConfigureAwait(false) : null;
        return user is not null
               && (await IsAdministrator(user).ConfigureAwait(false)
               || institutionId is not null && await IsOwnerOfInstitution(user, institutionId ?? Guid.Empty, cancellationToken).ConfigureAwait(false));
    }

    internal async Task<bool> IsAuthorizedToRevokeToken(
        ClaimsPrincipal claimsPrincipal,
        Guid tokenId,
        OpenIddictTokenManager<OpenIdConnectToken> tokenManager,
        CancellationToken cancellationToken)
    {
        var user = await GetUserAsync(claimsPrincipal);
        var token = await tokenManager.FindByIdAsync(tokenId.ToString(), cancellationToken);
        Guid? institutionId = token is not null && token.Application is not null ? await GetInstitutionIdByApplicationId(token.Application.Id, cancellationToken).ConfigureAwait(false) : null;
        return user is not null
               && (await IsAdministrator(user).ConfigureAwait(false)
               || institutionId is not null && await IsOwnerOfInstitution(user, institutionId ?? Guid.Empty, cancellationToken).ConfigureAwait(false));
    }

    private async Task<Guid?> GetInstitutionIdByApplicationId(Guid applicationId, CancellationToken cancellationToken)
    {
        return (
            await Context.InstitutionOpenIdConnectApplications.Where(x =>
                x.ApplicationId == applicationId
            ).Select(x => new
            {
                x.InstitutionId
            }
            ).SingleOrDefaultAsync(cancellationToken).ConfigureAwait(false)
        )?.InstitutionId;
    }
}