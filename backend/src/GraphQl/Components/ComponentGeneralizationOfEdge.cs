using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate.CostAnalysis.Types;
using Metabase.Authorization;
using Metabase.Data;
using Metabase.GraphQl.Users;

namespace Metabase.GraphQl.Components;

public sealed class ComponentGeneralizationOfEdge(
    ComponentConcretizationAndGeneralization association
)
: Edge<Component, IComponentByIdDataLoader>(association.ConcreteComponentId)
{
    [UseUserManager]
    [Cost(1)]
    public Task<bool> IsAuthorizedToRemoveEdgeAsync(
        ClaimsPrincipal claimsPrincipal,
        ComponentGeneralizationAuthorization authorization,
        CancellationToken cancellationToken
    )
    {
        return authorization.IsAuthorizedToManage(
            claimsPrincipal,
            association.GeneralComponentId,
            association.ConcreteComponentId,
            cancellationToken
        );
    }
}