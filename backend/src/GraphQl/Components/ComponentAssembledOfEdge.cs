using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using Metabase.Authorization;
using Metabase.Enumerations;
using Metabase.GraphQl.Users;
using Microsoft.AspNetCore.Identity;

namespace Metabase.GraphQl.Components;

public sealed class ComponentAssembledOfEdge
    : Edge<Data.Component, ComponentByIdDataLoader>
{
    private readonly Data.ComponentAssembly _association;

    public byte? Index => _association.Index;

    public PrimeSurface? PrimeSurface => _association.PrimeSurface;

    public ComponentAssembledOfEdge(
        Data.ComponentAssembly association
    )
        : base(association.PartComponentId)
    {
        _association = association;
    }

    [UseUserManager]
    public Task<bool> CanCurrentUserUpdateEdgeAsync(
        ClaimsPrincipal claimsPrincipal,
        [Service(ServiceKind.Resolver)] UserManager<Data.User> userManager,
        Data.ApplicationDbContext context,
        CancellationToken cancellationToken
    )
    {
        return ComponentAssemblyAuthorization.IsAuthorizedToManage(
            claimsPrincipal,
            _association.AssembledComponentId,
            _association.PartComponentId,
            userManager,
            context,
            cancellationToken
        );
    }

    [UseUserManager]
    public Task<bool> CanCurrentUserRemoveEdgeAsync(
        ClaimsPrincipal claimsPrincipal,
        [Service(ServiceKind.Resolver)] UserManager<Data.User> userManager,
        Data.ApplicationDbContext context,
        CancellationToken cancellationToken
    )
    {
        return ComponentAssemblyAuthorization.IsAuthorizedToManage(
            claimsPrincipal,
            _association.AssembledComponentId,
            _association.PartComponentId,
            userManager,
            context,
            cancellationToken
        );
    }
}