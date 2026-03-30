using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut.Data;
using HotChocolate.CostAnalysis.Types;
using Metabase.Authorization;
using Metabase.Data;
using Metabase.GraphQl.Users;

namespace Metabase.GraphQl.Components;

public sealed class ComponentManufacturerConnection(
    Component subject,
    QueryContext<ComponentManufacturer> queryContext
)
: Connection<Component, ComponentManufacturer, ComponentManufacturerEdge, IComponentManufacturersByComponentIdDataLoader>(
    subject,
    association => new ComponentManufacturerEdge(association),
    queryContext
)
{
    [UseUserManager]
    [Cost(1)]
    public Task<bool> IsAuthorizedToAddEdgeAsync(
        ClaimsPrincipal claimsPrincipal,
        ComponentManufacturerAuthorization authorization,
        CancellationToken cancellationToken
    )
    {
        return authorization.IsAuthorizedToAdd(
            claimsPrincipal,
            Subject.Id,
            cancellationToken
        );
    }
}

public sealed class PendingComponentManufacturerConnection(
    Component subject,
    QueryContext<ComponentManufacturer> queryContext
)
: Connection<Component, ComponentManufacturer, ComponentManufacturerEdge, IPendingComponentManufacturersByComponentIdDataLoader>(
    subject,
    association => new ComponentManufacturerEdge(association),
    queryContext
)
{
    [UseUserManager]
    [Cost(1)]
    public Task<bool> IsAuthorizedToAddEdgeAsync(
        ClaimsPrincipal claimsPrincipal,
        ComponentManufacturerAuthorization authorization,
        CancellationToken cancellationToken
    )
    {
        return authorization.IsAuthorizedToAdd(
            claimsPrincipal,
            Subject.Id,
            cancellationToken
        );
    }
}