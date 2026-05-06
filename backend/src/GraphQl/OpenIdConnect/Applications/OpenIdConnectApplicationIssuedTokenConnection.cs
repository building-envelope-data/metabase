using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate.CostAnalysis.Types;
using Metabase.Data.OpenIdConnect;
using OpenIddict.Core;

namespace Metabase.GraphQl.OpenIdConnect.Applications;

public sealed class OpenIdConnectApplicationIssuedTokenConnection(
    OpenIdConnectApplication application
)
{
    [Cost(0)]
    public async Task<uint> GetTotalCountAsync(
        ClaimsPrincipal claimsPrincipal,
        Authorization.OpenIdConnectAuthorization authorization,
        OpenIddictTokenManager<OpenIdConnectToken> tokenManager,
        CancellationToken cancellationToken
    )
    {
        if (!await authorization.IsAuthorizedToManageTokensOfApplication(claimsPrincipal, application.Id, cancellationToken))
        {
            return 0;
        }
        return (uint)await tokenManager.FindByApplicationIdAsync(application.Id.ToString(), cancellationToken).CountAsync(cancellationToken);
    }

    [Cost(0)]
    public async IAsyncEnumerable<OpenIdConnectApplicationIssuedTokenEdge> GetEdgesAsync(
        ClaimsPrincipal claimsPrincipal,
        Authorization.OpenIdConnectAuthorization authorization,
        OpenIddictTokenManager<OpenIdConnectToken> tokenManager,
        [EnumeratorCancellation] CancellationToken cancellationToken
    )
    {
        if (!await authorization.IsAuthorizedToManageTokensOfApplication(claimsPrincipal, application.Id, cancellationToken))
        {
            yield break;
        }
        await foreach (var token in tokenManager.FindByApplicationIdAsync(application.Id.ToString(), cancellationToken))
        {
            yield return new OpenIdConnectApplicationIssuedTokenEdge(token);
        }
    }
}