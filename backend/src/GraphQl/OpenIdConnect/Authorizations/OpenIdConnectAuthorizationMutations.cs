using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Metabase.Data.OpenIdConnect;
using Metabase.Extensions;
using Metabase.GraphQl.Users;
using OpenIddict.Core;

namespace Metabase.GraphQl.OpenIdConnect.Authorizations;

[ExtendObjectType(nameof(Mutation))]
public sealed class OpenIdConnectAuthorizationMutations
{
    [UseUserManager]
    [Authorize(Policy = Authorization.AuthorizationPolicies.ManageOpenIdConnectScopePolicy)]
    public async Task<DeleteOpenIdConnectAuthorizationPayload> DeleteOpenIdConnectAuthorizationAsync(
        DeleteOpenIdConnectAuthorizationInput input,
        ClaimsPrincipal claimsPrincipal,
        Authorization.OpenIdConnectAuthorization openIdConnectAuthorization,
        OpenIddictAuthorizationManager<OpenIdConnectAuthorization> authorizationManager,
        CancellationToken cancellationToken
    )
    {
        if (!await openIdConnectAuthorization.IsAuthorizedToManageAuthorization(
                claimsPrincipal,
                input.AuthorizationId,
                cancellationToken
            )
        )
        {
            return new DeleteOpenIdConnectAuthorizationPayload(
                new DeleteOpenIdConnectAuthorizationError(
                    DeleteOpenIdConnectAuthorizationErrorCode.UNAUTHORIZED,
                    "You are not authorized to delete the authorization.",
                    [nameof(input), nameof(input.AuthorizationId).FirstCharToLower()]
                )
            );
        }
        var authorization = await authorizationManager.FindByIdAsync(input.AuthorizationId.ToString(), cancellationToken);
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
        await authorizationManager.DeleteAsync(authorization, cancellationToken);
        return new DeleteOpenIdConnectAuthorizationPayload();
    }
}