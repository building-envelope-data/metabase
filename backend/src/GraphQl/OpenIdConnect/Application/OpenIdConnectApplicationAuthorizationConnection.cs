using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading;
using Metabase.Data.OpenIdConnect;
using OpenIddict.Core;

namespace Metabase.GraphQl.OpenIdConnect.Application;

public sealed class OpenIdConnectApplicationAuthorizationConnection(
    OpenIdConnectApplication application
)
{
    public async IAsyncEnumerable<OpenIdConnectApplicationAuthorizationEdge> GetEdgesAsync(
        ClaimsPrincipal claimsPrincipal,
        Authorization.OpenIdConnectAuthorization authorization,
        OpenIddictAuthorizationManager<OpenIdConnectAuthorization> authorizationManager,
        [EnumeratorCancellation] CancellationToken cancellationToken
    )
    {
        if (!await authorization.IsAuthorizedToManageAuthorizations(claimsPrincipal, application.Id, cancellationToken))
        {
            yield break;
        }
        await foreach (var auth in authorizationManager.FindByApplicationIdAsync(application.Id.ToString(), cancellationToken))
        {
            yield return new OpenIdConnectApplicationAuthorizationEdge(auth);
        }
    }
}