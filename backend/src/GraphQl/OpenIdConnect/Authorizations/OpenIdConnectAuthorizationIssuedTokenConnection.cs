using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate.CostAnalysis.Types;
using Metabase.Data.OpenIdConnect;
using OpenIddict.Core;

namespace Metabase.GraphQl.OpenIdConnect.Authorizations;

public sealed class OpenIdConnectAuthorizationIssuedTokenConnection(
    OpenIdConnectAuthorization authorization
)
{
    [Cost(0)]
    public async Task<int> GetTotalCountAsync(
        ClaimsPrincipal claimsPrincipal,
        Authorization.OpenIdConnectAuthorization auth,
        OpenIddictTokenManager<OpenIdConnectToken> tokenManager,
        CancellationToken cancellationToken
    )
    {
        if (!await auth.IsAuthorizedToManageTokensOfAuthorization(claimsPrincipal, authorization.Id, cancellationToken))
        {
            return 0;
        }
        return await tokenManager.FindByAuthorizationIdAsync(authorization.Id.ToString(), cancellationToken).CountAsync(cancellationToken);
    }

    [Cost(0)]
    public async IAsyncEnumerable<OpenIdConnectAuthorizationIssuedTokenEdge> GetEdgesAsync(
        ClaimsPrincipal claimsPrincipal,
        Authorization.OpenIdConnectAuthorization auth,
        OpenIddictTokenManager<OpenIdConnectToken> tokenManager,
        [EnumeratorCancellation] CancellationToken cancellationToken
    )
    {
        if (!await auth.IsAuthorizedToManageTokensOfAuthorization(claimsPrincipal, authorization.Id, cancellationToken))
        {
            yield break;
        }
        await foreach (var token in tokenManager.FindByAuthorizationIdAsync(authorization.Id.ToString(), cancellationToken))
        {
            yield return new OpenIdConnectAuthorizationIssuedTokenEdge(token);
        }
    }
}