using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut.Data;
using Metabase.Authorization;
using Metabase.Data;
using Metabase.GraphQl.Users;

namespace Metabase.GraphQl.Components;

public sealed class ComponentManufacturerConnection(
    Component subject,
    QueryContext<ComponentManufacturer> queryContext
    )
        : Connection<Component, ComponentManufacturer, ComponentManufacturersByComponentIdDataLoader, ComponentManufacturerEdge>(
        subject,
        x => new ComponentManufacturerEdge(x),
        queryContext
        )
{
    [UseUserManager]
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
        : AuthorizedConnection<Component, ComponentManufacturer, PendingComponentManufacturersByComponentIdDataLoader, ComponentManufacturerEdge, ComponentManufacturerAuthorization>(
        subject,
        x => new ComponentManufacturerEdge(x),
        (claimsPrincipal, component, authorization, cancellationToken) =>
            authorization.IsAuthorizedToAdd(claimsPrincipal, component.Id, cancellationToken),
        queryContext
        )
{
    [UseUserManager]
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