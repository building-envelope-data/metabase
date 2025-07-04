using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Metabase.Authorization;
using Metabase.Data;
using Metabase.GraphQl.Users;
using Microsoft.AspNetCore.Identity;

namespace Metabase.GraphQl.Components;

public sealed class ComponentManufacturerConnection(
    Component subject,
    bool pending
    )
        : ForkingConnection<Component, ComponentManufacturer,
        PendingComponentManufacturersByComponentIdDataLoader, ComponentManufacturersByComponentIdDataLoader,
        ComponentManufacturerEdge>(
        subject,
        pending,
        x => new ComponentManufacturerEdge(x)
        )
{
    [UseUserManager]
    public Task<bool> CanCurrentUserAddEdgeAsync(
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