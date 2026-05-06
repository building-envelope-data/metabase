using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut.Data;
using HotChocolate.CostAnalysis.Types;
using Metabase.Authorization;
using Metabase.Data;
using Metabase.GraphQl.Users;

namespace Metabase.GraphQl.Components;

public sealed class ComponentConcretizationOfConnection(
    Component subject,
    QueryContext<ComponentConcretizationAndGeneralization> queryContext
    )
        : Connection<Component, ComponentConcretizationAndGeneralization, ComponentConcretizationOfEdge, IComponentGeneralizationsByComponentIdDataLoader>(
        subject,
        x => new ComponentConcretizationOfEdge(x),
        queryContext
        )
{
    [UseUserManager]
    [Cost(1)]
    public Task<bool> IsAuthorizedToAddEdgeAsync(
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