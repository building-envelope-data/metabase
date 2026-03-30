using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate.CostAnalysis.Types;
using Metabase.Authorization;
using Metabase.Data;
using Metabase.Enumerations;
using Metabase.GraphQl.Users;

namespace Metabase.GraphQl.Components;

public sealed class ComponentAssembledOfEdge(
    ComponentAssembly association
)
: Edge<Component, IComponentByIdDataLoader>(association.PartComponentId)
{
    public byte? Index => association.Index;

    public PrimeSurface? PrimeSurface => association.PrimeSurface;

    [UseUserManager]
    [Cost(1)]
    public Task<bool> IsAuthorizedToUpdateEdgeAsync(
        ClaimsPrincipal claimsPrincipal,
        ComponentAssemblyAuthorization authorization,
        CancellationToken cancellationToken
    )
    {
        return authorization.IsAuthorizedToManage(
            claimsPrincipal,
            association.AssembledComponentId,
            association.PartComponentId,
            cancellationToken
        );
    }

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
            association.AssembledComponentId,
            association.PartComponentId,
            cancellationToken
        );
    }
}