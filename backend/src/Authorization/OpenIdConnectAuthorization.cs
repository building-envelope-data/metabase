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

public static class OpenIdConnectAuthorization
{
    public static async Task<bool> IsAuthorizedToViewApplications(
        ClaimsPrincipal claimsPrincipal,
        UserManager<User> userManager,
        ApplicationDbContext context,
        CancellationToken cancellationToken
    )
    {
        var user = await userManager.GetUserAsync(claimsPrincipal).ConfigureAwait(false);
        return user is not null
               && (await CommonAuthorization.IsAdministrator(user, userManager).ConfigureAwait(false)
               || await CommonAuthorization.IsOwner(user, context, cancellationToken).ConfigureAwait(false));
    }

    public static async Task<bool> IsAuthorizedToManageApplications(
        ClaimsPrincipal claimsPrincipal,
        UserManager<User> userManager,
        ApplicationDbContext context,
        CancellationToken cancellationToken)
    {
        var user = await userManager.GetUserAsync(claimsPrincipal).ConfigureAwait(false);
        return user is not null
               && (await CommonAuthorization.IsAdministrator(user, userManager).ConfigureAwait(false)
               || await CommonAuthorization.IsOwner(user, context, cancellationToken).ConfigureAwait(false));
    }

    public static async Task<bool> IsAuthorizedToManageApplication(
        Guid applicationId,
        ClaimsPrincipal claimsPrincipal,
        UserManager<User> userManager,
        ApplicationDbContext context,
        CancellationToken cancellationToken)
    {
        var user = await userManager.GetUserAsync(claimsPrincipal).ConfigureAwait(false);
        var institutionId = await GetInstitutionIdByApplicationId(applicationId, context, cancellationToken).ConfigureAwait(false);
        return user is not null
               && (await CommonAuthorization.IsAdministrator(user, userManager).ConfigureAwait(false)
               || await CommonAuthorization.IsOwnerOfInstitution(user, institutionId, context, cancellationToken).ConfigureAwait(false));
    }

    public static async Task<bool> IsAuthorizedToDeleteAuthorization(
        Guid authorizationId,
        OpenIddictAuthorizationManager<Data.OpenIdConnect.OpenIdConnectAuthorization> authorizationManager,
        ClaimsPrincipal claimsPrincipal,
        UserManager<User> userManager,
        ApplicationDbContext context,
        CancellationToken cancellationToken)
    {
        var user = await userManager.GetUserAsync(claimsPrincipal).ConfigureAwait(false);
        var authorization = await authorizationManager.FindByIdAsync(authorizationId.ToString(), cancellationToken).ConfigureAwait(false);
        Guid? institutionId = authorization is not null && authorization.Application is not null ? await GetInstitutionIdByApplicationId(authorization.Application.Id, context, cancellationToken).ConfigureAwait(false) : null;
        return user is not null
               && (await CommonAuthorization.IsAdministrator(user, userManager).ConfigureAwait(false)
               || institutionId is not null && await CommonAuthorization.IsOwnerOfInstitution(user, institutionId ?? Guid.Empty, context, cancellationToken).ConfigureAwait(false));
    }

    public static async Task<bool> IsAuthorizedToRevokeToken(
        Guid tokenId,
        OpenIddictTokenManager<OpenIdConnectToken> tokenManager,
        ClaimsPrincipal claimsPrincipal,
        UserManager<User> userManager,
        ApplicationDbContext context,
        CancellationToken cancellationToken)
    {
        var user = await userManager.GetUserAsync(claimsPrincipal).ConfigureAwait(false);
        var token = await tokenManager.FindByIdAsync(tokenId.ToString(), cancellationToken).ConfigureAwait(false);
        Guid? institutionId = token is not null && token.Application is not null ? await GetInstitutionIdByApplicationId(token.Application.Id, context, cancellationToken).ConfigureAwait(false) : null;
        return user is not null
               && (await CommonAuthorization.IsAdministrator(user, userManager).ConfigureAwait(false)
               || institutionId is not null && await CommonAuthorization.IsOwnerOfInstitution(user, institutionId ?? Guid.Empty, context, cancellationToken).ConfigureAwait(false));
    }

    private static async Task<Guid> GetInstitutionIdByApplicationId(Guid applicationId, ApplicationDbContext context, CancellationToken cancellationToken)
    {
        return (
            await context.InstitutionOpenIdConnectApplications.Where(x =>
                x.ApplicationId == applicationId
            ).Select(x => new
            {
                x.InstitutionId
            }
            ).SingleAsync(cancellationToken).ConfigureAwait(false)
        ).InstitutionId;
    }
}