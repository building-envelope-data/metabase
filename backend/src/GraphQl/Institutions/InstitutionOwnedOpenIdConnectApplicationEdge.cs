using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate.CostAnalysis.Types;
using Metabase.Data.OpenIdConnect;
using Metabase.GraphQl.Users;

namespace Metabase.GraphQl.Institutions;

public sealed class InstitutionOwnedOpenIdConnectApplicationEdge(
    OpenIdConnectApplication node
    )
{
    public OpenIdConnectApplication Node { get; } = node;

    [UseUserManager]
    [Cost(1)]
    public Task<bool> IsAuthorizedToRemoveEdgeAsync(
        ClaimsPrincipal claimsPrincipal,
        Authorization.OpenIdConnectAuthorization authorization,
        CancellationToken cancellationToken
    )
    {
        return authorization.IsAuthorizedToManageApplications(
            claimsPrincipal,
            Node.OwnerId,
            cancellationToken
        );
    }
}