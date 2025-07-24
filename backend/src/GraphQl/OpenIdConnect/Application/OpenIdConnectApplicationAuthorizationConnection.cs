using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Metabase.Data.OpenIdConnect;
using OpenIddict.Core;

namespace Metabase.GraphQl.OpenIdConnect.Application;

public sealed class OpenIdConnectApplicationAuthorizationConnection(
    OpenIdConnectApplication application
)
{
    public IAsyncEnumerable<OpenIdConnectApplicationAuthorizationEdge> GetEdgesAsync(
        OpenIddictAuthorizationManager<OpenIdConnectAuthorization> authorizationManager,
        CancellationToken cancellationToken
    )
    {
        return authorizationManager.FindByApplicationIdAsync(application.Id.ToString(), cancellationToken)
            .Select(authorization => new OpenIdConnectApplicationAuthorizationEdge(authorization));
    }
}