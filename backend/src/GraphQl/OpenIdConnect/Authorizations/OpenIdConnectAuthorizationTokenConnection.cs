using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading;
using Metabase.Data.OpenIdConnect;
using OpenIddict.Core;

namespace Metabase.GraphQl.OpenIdConnect.Authorizations;

public sealed class OpenIdConnectAuthorizationTokenConnection(
    OpenIdConnectAuthorization authorization
)
{
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