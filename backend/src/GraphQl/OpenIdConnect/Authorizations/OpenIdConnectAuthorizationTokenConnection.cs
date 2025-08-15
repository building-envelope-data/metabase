using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Metabase.Data.OpenIdConnect;
using OpenIddict.Core;

namespace Metabase.GraphQl.OpenIdConnect.Authorizations;

public sealed class OpenIdConnectAuthorizationTokenConnection(
    OpenIdConnectAuthorization authorization
)
{
    public async Task<uint> GetTotalCountAsync(
        ClaimsPrincipal claimsPrincipal,
        Authorization.OpenIdConnectAuthorization auth,
        OpenIddictAuthorizationManager<OpenIdConnectAuthorization> authorizationManager,
        OpenIddictTokenManager<OpenIdConnectToken> tokenManager,
        CancellationToken cancellationToken
    )
    {
        if (!await auth.IsAuthorizedToManageTokensOfAuthorization(claimsPrincipal, authorization.Id, authorizationManager, cancellationToken))
        {
            return 0;
        }
        return (uint)await tokenManager.FindByAuthorizationIdAsync(authorization.Id.ToString(), cancellationToken).CountAsync(cancellationToken);
    }

    public async IAsyncEnumerable<OpenIdConnectAuthorizationTokenEdge> GetEdgesAsync(
        ClaimsPrincipal claimsPrincipal,
        Authorization.OpenIdConnectAuthorization auth,
        OpenIddictAuthorizationManager<OpenIdConnectAuthorization> authorizationManager,
        OpenIddictTokenManager<OpenIdConnectToken> tokenManager,
        [EnumeratorCancellation] CancellationToken cancellationToken
    )
    {
        if (!await auth.IsAuthorizedToManageTokensOfAuthorization(claimsPrincipal, authorization.Id, authorizationManager, cancellationToken))
        {
            yield break;
        }
        await foreach (var token in tokenManager.FindByAuthorizationIdAsync(authorization.Id.ToString(), cancellationToken))
        {
            yield return new OpenIdConnectAuthorizationTokenEdge(token);
        }
    }
}