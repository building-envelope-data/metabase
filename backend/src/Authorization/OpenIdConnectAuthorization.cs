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
using System.Runtime.CompilerServices;

namespace Metabase.Authorization;

public sealed class OpenIdConnectAuthorization(
    ApplicationDbContext context,
    UserManager<User> userManager,
    OpenIddictApplicationManager<OpenIdConnectApplication> applicationManager
) : CommonAuthorization(context, userManager, applicationManager)
{
    internal Task<bool> IsAuthorizedToManage(
        ClaimsPrincipal claimsPrincipal,
        CancellationToken cancellationToken
    )
    {
        return AuthorizeAsync(
            claimsPrincipal,
            IsAdministrator,
            application => Task.FromResult(false),
            cancellationToken
        );
    }

    internal Task<bool> IsAuthorizedToManageApplications(
        ClaimsPrincipal claimsPrincipal,
        Guid institutionId,
        CancellationToken cancellationToken
    )
    {
        return AuthorizeAsync(
            claimsPrincipal,
            user => IsAtLeastAssistantOfVerifiedInstitution(user, institutionId, cancellationToken),
            application => BelongsToVerifiedInstitution(application, institutionId, cancellationToken),
            cancellationToken
        );
    }

    internal Task<bool> IsAuthorizedToManageApplication(
        ClaimsPrincipal claimsPrincipal,
        Guid applicationId,
        CancellationToken cancellationToken
    )
    {
        return AuthorizeAsync(
            claimsPrincipal,
            async user =>
            {
                var institutionIds = GetInstitutionIdsByApplicationId(applicationId);
                return await IsOwnerOfAtLeastOneInstitution(user, institutionIds, cancellationToken);
            },
            application => BelongsToAtLeastOneInstitutionOfApplication(application, applicationId, cancellationToken),
            cancellationToken
        );
    }

    internal Task<bool> IsAuthorizedToManageAuthorizations(
        ClaimsPrincipal claimsPrincipal,
        Guid applicationId,
        CancellationToken cancellationToken
    )
    {
        return IsAuthorizedToManageApplication(claimsPrincipal, applicationId, cancellationToken);
    }

    internal Task<bool> IsAuthorizedToManageAuthorization(
        ClaimsPrincipal claimsPrincipal,
        Guid authorizationId,
        OpenIddictAuthorizationManager<Data.OpenIdConnect.OpenIdConnectAuthorization> authorizationManager,
        CancellationToken cancellationToken
    )
    {
        return AuthorizeAsync(
            claimsPrincipal,
            async user =>
            {
                var authorization = await authorizationManager.FindByIdAsync(authorizationId.ToString(), cancellationToken);
                var institutionIds = authorization is not null && authorization.Application is not null
                    ? GetInstitutionIdsByApplicationId(authorization.Application.Id)
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

    internal Task<bool> IsAuthorizedToManageTokens(
        ClaimsPrincipal claimsPrincipal,
        Guid applicationId,
        CancellationToken cancellationToken
    )
    {
        return IsAuthorizedToManageApplication(claimsPrincipal, applicationId, cancellationToken);
    }

    internal Task<bool> IsAuthorizedToManageToken(
        ClaimsPrincipal claimsPrincipal,
        Guid tokenId,
        OpenIddictTokenManager<OpenIdConnectToken> tokenManager,
        CancellationToken cancellationToken
    )
    {
        return AuthorizeAsync(
            claimsPrincipal,
            async user =>
            {
                var token = await tokenManager.FindByIdAsync(tokenId.ToString(), cancellationToken);
                var institutionIds = token is not null && token.Application is not null
                    ? GetInstitutionIdsByApplicationId(token.Application.Id)
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

    private async IAsyncEnumerable<Guid> GetInstitutionIdsByApplicationId(
        Guid applicationId
    )
    {
        await foreach (
            var x in Context.InstitutionOpenIdConnectApplications.AsNoTracking()
                .Where(x => x.ApplicationId == applicationId)
                .Select(x => new { x.InstitutionId })
                .ToAsyncEnumerable()
        )
        {
            yield return x.InstitutionId;
        }
    }

    private async Task<bool> IsOwnerOfAtLeastOneInstitution(
        User user,
        IAsyncEnumerable<Guid> institutionIds,
        CancellationToken cancellationToken
    )
    {
        await foreach (var institutionId in institutionIds)
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
            .Where(a => a.ApplicationId == applicationId)
            .Where(a =>
                a.Institution.OpenIdConnectApplicationEdges.Any(e => e.ApplicationId == application.Id)
                || a.Institution.Manager != null && a.Institution.Manager.OpenIdConnectApplicationEdges.Any(e => e.ApplicationId == application.Id)
                || a.Institution.Manager != null && a.Institution.Manager.Manager != null && a.Institution.Manager.Manager.OpenIdConnectApplicationEdges.Any(e => e.ApplicationId == application.Id)
            )
            .AnyAsync(cancellationToken);
    }
}