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
using System.Collections.Generic;

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
                var institutionIds = await GetInstitutionIdsByApplicationId(applicationId, cancellationToken);
                return institutionIds is not null && await IsOwnerOfAtLeastOneInstitution(user, institutionIds, cancellationToken);
            },
            application => BelongsToAtLeastOneInstitutionOfApplication(application, applicationId, cancellationToken),
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
                var institutionIds = authorization is not null && authorization.Application is not null
                    ? await GetInstitutionIdsByApplicationId(authorization.Application.Id, cancellationToken)
                    : null;
                return institutionIds is not null
                    && await IsOwnerOfAtLeastOneInstitution(user, institutionIds, cancellationToken);
            },
            async application =>
            {
                var authorization = await authorizationManager.FindByIdAsync(authorizationId.ToString(), cancellationToken);
                return authorization is not null
                    && authorization.Application is not null
                    && await BelongsToAtLeastOneInstitutionOfApplication(application, authorization.Application.Id, cancellationToken);
            },
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
                var institutionIds = token is not null && token.Application is not null
                    ? await GetInstitutionIdsByApplicationId(token.Application.Id, cancellationToken)
                    : null;
                return institutionIds is not null && await IsOwnerOfAtLeastOneInstitution(user, institutionIds, cancellationToken);
            },
            async application =>
            {
                var token = await tokenManager.FindByIdAsync(tokenId.ToString(), cancellationToken);
                return token is not null
                    && token.Application is not null
                    && await BelongsToAtLeastOneInstitutionOfApplication(application, token.Application.Id, cancellationToken);
            },
            cancellationToken
        );
    }

    private async Task<IEnumerable<Guid>> GetInstitutionIdsByApplicationId(Guid applicationId, CancellationToken cancellationToken)
    {
        return (
            await Context.InstitutionOpenIdConnectApplications.AsNoTracking()
                .Where(x => x.ApplicationId == applicationId)
                .Select(x => new { x.InstitutionId })
                .ToListAsync(cancellationToken)
        ).Select(x => x.InstitutionId);
    }

    private async Task<bool> IsOwnerOfAtLeastOneInstitution(
        User user,
        IEnumerable<Guid> institutionIds,
        CancellationToken cancellationToken
    )
    {
        foreach (var institutionId in institutionIds)
        {
            if (await IsOwnerOfInstitution(user, institutionId, cancellationToken))
            {
                return true;
            }
        }
        return false;
    }

    private Task<bool> BelongsToAtLeastOneInstitutionOfApplication(
        OpenIdConnectApplication application,
        Guid applicationId,
        CancellationToken cancellationToken
    )
    {
        return Context.InstitutionOpenIdConnectApplications.AsNoTracking()
            .Where(a => a.ApplicationId == application.Id)
            .Where(a => a.Institution.OpenIdConnectApplicationEdges.Any(e => e.ApplicationId == applicationId))
            .AnyAsync(cancellationToken);
    }
}