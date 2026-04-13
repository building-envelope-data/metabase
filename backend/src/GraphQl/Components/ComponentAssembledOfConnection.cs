using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut.Data;
using Metabase.Authorization;
using Metabase.Data;
using Metabase.GraphQl.Users;

namespace Metabase.GraphQl.Components;

public sealed class ComponentAssembledOfConnection(
    Component subject,
    QueryContext<ComponentAssembly> queryContext
    )
        : Connection<Component, ComponentAssembly, ComponentPartsByComponentIdDataLoader,
        ComponentAssembledOfEdge>(
        subject,
        x => new ComponentAssembledOfEdge(x),
        queryContext
        )
{
    [UseUserManager]
    public Task<bool> IsAuthorizedToAddEdgeAsync(
        ClaimsPrincipal claimsPrincipal,
        ComponentAssemblyAuthorization authorization,
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