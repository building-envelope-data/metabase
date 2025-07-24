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

namespace Metabase.GraphQl.OpenIdConnect.Tokens;

[ExtendObjectType(nameof(Mutation))]
public sealed class OpenIdConnectTokenMutations
{
    [UseUserManager]
    [Authorize(Policy = AuthConfiguration.WritePolicy)]
    public async Task<RevokeOpenIdConnectTokenPayload> RevokeOpenIdConnectTokenAsync(
        RevokeOpenIdConnectTokenInput input,
        ClaimsPrincipal claimsPrincipal,
        Authorization.OpenIdConnectAuthorization authorization,
        OpenIddictTokenManager<OpenIdConnectToken> tokenManager,
        ApplicationDbContext context,
        CancellationToken cancellationToken
    )
    {
        if (!await authorization.IsAuthorizedToRevokeToken(
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
                    "Unknown Token.",
                    [nameof(input), nameof(input.TokenId).FirstCharToLower()]
                )
            );
        }
        await tokenManager.TryRevokeAsync(token, cancellationToken);
        return new RevokeOpenIdConnectTokenPayload();
    }
}