using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Metabase.Authorization;
using Metabase.Data;
using Metabase.Enumerations;
using Metabase.GraphQl.Users;
using Microsoft.AspNetCore.Identity;

namespace Metabase.GraphQl.Components;

public sealed class ComponentAssembledOfEdge(
    ComponentAssembly association
    )
        : Edge<Component, ComponentByIdDataLoader>(association.PartComponentId)
{
    private readonly ComponentAssembly _association = association;

    public byte? Index => _association.Index;

    public PrimeSurface? PrimeSurface => _association.PrimeSurface;

    [UseUserManager]
    public Task<bool> CanCurrentUserUpdateEdgeAsync(
        ClaimsPrincipal claimsPrincipal,
        ComponentAssemblyAuthorization authorization,
        CancellationToken cancellationToken
    )
    {
        return authorization.IsAuthorizedToManage(
            claimsPrincipal,
            _association.AssembledComponentId,
            _association.PartComponentId,
            cancellationToken
        );
    }

    [UseUserManager]
    public Task<bool> CanCurrentUserRemoveEdgeAsync(
        ClaimsPrincipal claimsPrincipal,
        ComponentAssemblyAuthorization authorization,
        CancellationToken cancellationToken
    )
    {
        return authorization.IsAuthorizedToManage(
            claimsPrincipal,
            _association.AssembledComponentId,
            _association.PartComponentId,
            cancellationToken
        );
    }
}