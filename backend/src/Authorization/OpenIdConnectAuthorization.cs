using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Metabase.Data;
using Metabase.Data.OpenIdConnect;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Core;

namespace Metabase.Authorization;

public sealed class OpenIdConnectAuthorization(
    IDbContextFactory<ApplicationDbContext> dbContextFactory,
    UserManager<User> userManager,
    OpenIddictApplicationManager<OpenIdConnectApplication> applicationManager
) : CommonAuthorization(dbContextFactory, userManager, applicationManager)
{
    internal Task<bool> IsAuthorizedToManageOpenIdConnect(
        ClaimsPrincipal claimsPrincipal,
        CancellationToken cancellationToken
    )
    {
        return AuthorizeAsync(
            claimsPrincipal,
            user => CanAdministrate(user, claimsPrincipal),
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
                var application = await ApplicationManager.FindByIdAsync(applicationId.ToString());
                return application is not null && await IsOwnerOfInstitution(user, application.OwnerId, cancellationToken);
            },
            application => BelongsToApplicationOwner(application, applicationId, cancellationToken),
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

    internal async Task<bool> IsAuthorizedToManageAuthorization(
        ClaimsPrincipal claimsPrincipal,
        Guid authorizationId,
        OpenIddictAuthorizationManager<Data.OpenIdConnect.OpenIdConnectAuthorization> authorizationManager,
        CancellationToken cancellationToken
    )
    {
        var authorization = await authorizationManager.FindByIdAsync(authorizationId.ToString(), cancellationToken);
        return
            authorization is not null
            && authorization.Application is not null
            && await IsAuthorizedToManageApplication(claimsPrincipal, authorization.Application.Id, cancellationToken);
    }

    internal Task<bool> IsAuthorizedToManageTokensOfApplication(
        ClaimsPrincipal claimsPrincipal,
        Guid applicationId,
        CancellationToken cancellationToken
    )
    {
        return IsAuthorizedToManageApplication(claimsPrincipal, applicationId, cancellationToken);
    }

    internal Task<bool> IsAuthorizedToManageTokensOfAuthorization(
        ClaimsPrincipal claimsPrincipal,
        Guid authorizationId,
        OpenIddictAuthorizationManager<Data.OpenIdConnect.OpenIdConnectAuthorization> authorizationManager,
        CancellationToken cancellationToken
    )
    {
        return IsAuthorizedToManageAuthorization(claimsPrincipal, authorizationId, authorizationManager, cancellationToken);
    }

    internal async Task<bool> IsAuthorizedToManageToken(
        ClaimsPrincipal claimsPrincipal,
        Guid tokenId,
        OpenIddictTokenManager<OpenIdConnectToken> tokenManager,
        CancellationToken cancellationToken
    )
    {
        var token = await tokenManager.FindByIdAsync(tokenId.ToString(), cancellationToken);
        return
            token is not null
            && token.Application is not null
            && await IsAuthorizedToManageApplication(claimsPrincipal, token.Application.Id, cancellationToken);
    }

    private Task<bool> BelongsToApplicationOwner(
        OpenIdConnectApplication application,
        Guid applicationId,
        CancellationToken cancellationToken
    )
    {
        return Context.OpenIdConnectApplications.AsNoTracking()
            .Where(a => a.Id == applicationId)
            .Where(a =>
                a.Owner.Id == application.OwnerId
                || a.Owner.ManagerId == application.OwnerId
                || a.Owner.Manager != null && a.Owner.Manager.ManagerId == application.OwnerId
            )
            .AnyAsync(cancellationToken);
    }
}