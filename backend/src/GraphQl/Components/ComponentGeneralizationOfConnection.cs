using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut.Data;
using Metabase.Authorization;
using Metabase.Data;
using Metabase.GraphQl.Users;
using Microsoft.AspNetCore.Identity;

namespace Metabase.GraphQl.Components;

public sealed class ComponentGeneralizationOfConnection(
    Component subject,
    QueryContext<ComponentConcretizationAndGeneralization> queryContext
    )
        : Connection<Component, ComponentConcretizationAndGeneralization,
        ComponentConcretizationsByComponentIdDataLoader, ComponentGeneralizationOfEdge>(
        subject,
        x => new ComponentGeneralizationOfEdge(x),
        queryContext
        )
{
    [UseUserManager]
    public Task<bool> CanCurrentUserAddEdgeAsync(
        ClaimsPrincipal claimsPrincipal,
        ComponentGeneralizationAuthorization authorization,
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