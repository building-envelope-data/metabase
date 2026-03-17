using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Metabase.Authorization;
using Metabase.Data.OpenIdConnect;
using Metabase.Extensions;
using Metabase.GraphQl.Users;
using OpenIddict.Core;

namespace Metabase.GraphQl.OpenIdConnect.Tokens;

[ExtendObjectType(nameof(Mutation))]
public sealed class OpenIdConnectTokenMutations
{
    [UseUserManager]
    [Authorize(Policy = AuthorizationPolicies.ManageOpenIdConnectScopePolicy)]
    public async Task<RevokeOpenIdConnectTokenPayload> RevokeOpenIdConnectTokenAsync(
        RevokeOpenIdConnectTokenInput input,
        ClaimsPrincipal claimsPrincipal,
        Authorization.OpenIdConnectAuthorization authorization,
        OpenIddictTokenManager<OpenIdConnectToken> tokenManager,
        CancellationToken cancellationToken
    )
    {
        if (!await authorization.IsAuthorizedToManageToken(
                claimsPrincipal,
                input.TokenId,
                tokenManager,
                cancellationToken
            ))
        {
            return new RevokeOpenIdConnectTokenPayload(
                new RevokeOpenIdConnectTokenError(
                    RevokeOpenIdConnectTokenErrorCode.UNAUTHORIZED,
                    "You are not authorized to revoke the token.",
                    [nameof(input), nameof(input.TokenId).FirstCharToLower()]
                )
            );
        }
        var token = await tokenManager.FindByIdAsync(input.TokenId.ToString(), cancellationToken);
        if (token is null)
        {
            return new RevokeOpenIdConnectTokenPayload(
                new RevokeOpenIdConnectTokenError(
                    RevokeOpenIdConnectTokenErrorCode.UNKNOWN_TOKEN,
                    "Unknown token.",
                    [nameof(input), nameof(input.TokenId).FirstCharToLower()]
                )
            );
        }
        if (!await tokenManager.TryRevokeAsync(token, cancellationToken))
        {
            return new RevokeOpenIdConnectTokenPayload(
                new RevokeOpenIdConnectTokenError(
                    RevokeOpenIdConnectTokenErrorCode.FAILED,
                    "Failed to revoke the token.",
                    [nameof(input), nameof(input.TokenId).FirstCharToLower()]
                )
            );
        }
        return new RevokeOpenIdConnectTokenPayload();
    }
}