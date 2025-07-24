using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Metabase.Data.OpenIdConnect;
using OpenIddict.Core;

namespace Metabase.GraphQl.OpenIdConnect.Application;

public sealed class OpenIdConnectApplicationTokenConnection(
    OpenIdConnectApplication application
)
{
    public IAsyncEnumerable<OpenIdConnectApplicationTokenEdge> GetEdgesAsync(
        OpenIddictTokenManager<OpenIdConnectToken> tokenManager,
        CancellationToken cancellationToken
    )
    {
        return tokenManager.FindByApplicationIdAsync(application.Id.ToString(), cancellationToken)
            .Select(token => new OpenIdConnectApplicationTokenEdge(token));
    }
}