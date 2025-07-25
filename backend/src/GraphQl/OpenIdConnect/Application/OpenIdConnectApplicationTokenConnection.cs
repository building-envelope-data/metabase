using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading;
using Metabase.Data.OpenIdConnect;
using OpenIddict.Core;

namespace Metabase.GraphQl.OpenIdConnect.Application;

public sealed class OpenIdConnectApplicationTokenConnection(
    OpenIdConnectApplication application
)
{
    public async IAsyncEnumerable<OpenIdConnectApplicationTokenEdge> GetEdgesAsync(
        ClaimsPrincipal claimsPrincipal,
        Authorization.OpenIdConnectAuthorization authorization,
        OpenIddictTokenManager<OpenIdConnectToken> tokenManager,
        [EnumeratorCancellation] CancellationToken cancellationToken
    )
    {
        if (!await authorization.IsAuthorizedToManage(claimsPrincipal, cancellationToken))
        {
            yield break;
        }
        await foreach (var token in tokenManager.FindByApplicationIdAsync(application.Id.ToString(), cancellationToken))
        {
            yield return new OpenIdConnectApplicationTokenEdge(token);
        }
    }
}