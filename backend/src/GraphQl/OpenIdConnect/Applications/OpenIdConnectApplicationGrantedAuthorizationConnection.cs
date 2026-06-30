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

public sealed class OpenIdConnectApplicationGrantedAuthorizationConnection(
    OpenIdConnectApplication application
)
{
    [Cost(0)]
    public async Task<int> GetTotalCountAsync(
        ClaimsPrincipal claimsPrincipal,
        Authorization.OpenIdConnectAuthorization authorization,
        OpenIddictAuthorizationManager<OpenIdConnectAuthorization> authorizationManager,
        CancellationToken cancellationToken
    )
    {
        if (!await authorization.IsAuthorizedToManageAuthorizations(claimsPrincipal, application.Id, cancellationToken))
        {
            return 0;
        }
        return await authorizationManager.FindByApplicationIdAsync(application.Id.ToString(), cancellationToken).CountAsync(cancellationToken);
    }

    [Cost(0)]
    public async IAsyncEnumerable<OpenIdConnectApplicationGrantedAuthorizationEdge> GetEdgesAsync(
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
            yield return new OpenIdConnectApplicationGrantedAuthorizationEdge(auth);
        }
    }
}