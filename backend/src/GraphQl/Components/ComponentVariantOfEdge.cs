using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate.CostAnalysis.Types;
using Metabase.Authorization;
using Metabase.Data;
using Metabase.GraphQl.Users;

namespace Metabase.GraphQl.Components;

public sealed class ComponentVariantOfEdge(
    ComponentVariant association
    )
        : Edge<Component, IComponentByIdDataLoader>(association.OfComponentId)
{
    [UseUserManager]
    [Cost(1)]
    public Task<bool> IsAuthorizedToRemoveEdgeAsync(
        ClaimsPrincipal claimsPrincipal,
        ComponentAssemblyAuthorization authorization,
        CancellationToken cancellationToken
    )
    {
        return authorization.IsAuthorizedToManage(
            claimsPrincipal,
            association.OfComponentId,
            association.ToComponentId,
            cancellationToken
        );
    }
}