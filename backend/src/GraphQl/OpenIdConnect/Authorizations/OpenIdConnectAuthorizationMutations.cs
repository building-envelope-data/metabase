using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Metabase.Configuration;
using Metabase.Data;
using Metabase.Data.OpenIdConnect;
using Metabase.Extensions;
using Metabase.GraphQl.Users;
using Microsoft.AspNetCore.Identity;
using OpenIddict.Core;

namespace Metabase.GraphQl.OpenIdConnect.Authorizations;

[ExtendObjectType(nameof(Mutation))]
public sealed class OpenIdConnectAuthorizationMutations
{
    [UseUserManager]
    [Authorize(Policy = AuthConfiguration.WritePolicy)]
    public async Task<DeleteOpenIdConnectAuthorizationPayload> DeleteOpenIdConnectAuthorizationAsync(
        DeleteOpenIdConnectAuthorizationInput input,
        ClaimsPrincipal claimsPrincipal,
        UserManager<User> userManager,
        OpenIddictAuthorizationManager<OpenIdConnectAuthorization> authorizationManager,
        ApplicationDbContext context,
        CancellationToken cancellationToken
    )
    {
        if (!await Authorization.OpenIdConnectAuthorization.IsAuthorizedToDeleteAuthorization(
                input.AuthorizationId,
                authorizationManager,
                claimsPrincipal,
                userManager,
                context,
                cancellationToken).ConfigureAwait(false))
        {
            return new DeleteOpenIdConnectAuthorizationPayload(
                new DeleteOpenIdConnectAuthorizationError(
                    DeleteOpenIdConnectAuthorizationErrorCode.UNAUTHORIZED,
                    "You are not authorized to delete the authorization.",
                    [nameof(input), nameof(input.AuthorizationId).FirstCharToLower()]
                )
            );
        }
        var authorization = await authorizationManager.FindByIdAsync(input.AuthorizationId.ToString(), cancellationToken).ConfigureAwait(false);
        if (authorization is null)
        {
            return new DeleteOpenIdConnectAuthorizationPayload(
                new DeleteOpenIdConnectAuthorizationError(
                    DeleteOpenIdConnectAuthorizationErrorCode.UNKNOWN_AUTHORIZATION,
                    "Unknown authorization.",
                    [nameof(input), nameof(input.AuthorizationId).FirstCharToLower()]
                )
            );
        }
        await authorizationManager.DeleteAsync(authorization, cancellationToken).ConfigureAwait(false);
        return new DeleteOpenIdConnectAuthorizationPayload();
    }
}