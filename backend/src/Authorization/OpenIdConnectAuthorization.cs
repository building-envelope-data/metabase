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
    UserManager<User> userManager,
    OpenIddictApplicationManager<OpenIdConnectApplication> applicationManager
) : CommonAuthorization(context, userManager, applicationManager)
{
    internal Task<bool> IsAuthorizedToViewApplications(
        ClaimsPrincipal claimsPrincipal,
        CancellationToken cancellationToken
    )
    {
        return AuthorizeAsync(
            claimsPrincipal,
            user => IsOwner(user, cancellationToken),
            application => Task.FromResult(false),
            cancellationToken
        );
    }

    internal Task<bool> IsAuthorizedToManageApplications(
        ClaimsPrincipal claimsPrincipal,
        CancellationToken cancellationToken)
    {
        return AuthorizeAsync(
            claimsPrincipal,
            user => IsOwner(user, cancellationToken),
            application => Task.FromResult(false),
            cancellationToken
        );
    }

    internal Task<bool> IsAuthorizedToManageApplication(
        ClaimsPrincipal claimsPrincipal,
        Guid applicationId,
        CancellationToken cancellationToken)
    {
        return AuthorizeAsync(
            claimsPrincipal,
            async user =>
            {
                var institutionId = await GetInstitutionIdByApplicationId(applicationId, cancellationToken);
                return institutionId is not null && await IsOwnerOfInstitution(user, institutionId ?? Guid.Empty, cancellationToken).ConfigureAwait(false);
            },
            application => Task.FromResult(false),
            cancellationToken
        );
    }

    internal Task<bool> IsAuthorizedToDeleteAuthorization(
        ClaimsPrincipal claimsPrincipal,
        Guid authorizationId,
        OpenIddictAuthorizationManager<Data.OpenIdConnect.OpenIdConnectAuthorization> authorizationManager,
        CancellationToken cancellationToken)
    {
        return AuthorizeAsync(
            claimsPrincipal,
            async user =>
            {
                var authorization = await authorizationManager.FindByIdAsync(authorizationId.ToString(), cancellationToken);
                var institutionId = authorization is not null && authorization.Application is not null ? await GetInstitutionIdByApplicationId(authorization.Application.Id, cancellationToken).ConfigureAwait(false) : null;
                return institutionId is not null && await IsOwnerOfInstitution(user, institutionId ?? Guid.Empty, cancellationToken).ConfigureAwait(false);
            },
            application => Task.FromResult(false),
            cancellationToken
        );
    }

    internal Task<bool> IsAuthorizedToRevokeToken(
        ClaimsPrincipal claimsPrincipal,
        Guid tokenId,
        OpenIddictTokenManager<OpenIdConnectToken> tokenManager,
        CancellationToken cancellationToken)
    {
        return AuthorizeAsync(
            claimsPrincipal,
            async user =>
            {
                var token = await tokenManager.FindByIdAsync(tokenId.ToString(), cancellationToken);
                var institutionId = token is not null && token.Application is not null ? await GetInstitutionIdByApplicationId(token.Application.Id, cancellationToken).ConfigureAwait(false) : null;
                return institutionId is not null && await IsOwnerOfInstitution(user, institutionId ?? Guid.Empty, cancellationToken).ConfigureAwait(false);
            },
            application => Task.FromResult(false),
            cancellationToken
        );
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