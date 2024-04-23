using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using Metabase.Authorization;
using Metabase.GraphQl.Users;
using Microsoft.AspNetCore.Identity;

namespace Metabase.GraphQl.Components;

public sealed class ComponentConcretizationOfEdge
    : Edge<Data.Component, ComponentByIdDataLoader>
{
    private readonly Data.ComponentConcretizationAndGeneralization _association;

    public ComponentConcretizationOfEdge(
        Data.ComponentConcretizationAndGeneralization association
    )
        : base(association.GeneralComponentId)
    {
        _association = association;
    }

    [UseUserManager]
    public Task<bool> CanCurrentUserRemoveEdgeAsync(
        ClaimsPrincipal claimsPrincipal,
        [Service(ServiceKind.Resolver)] UserManager<Data.User> userManager,
        Data.ApplicationDbContext context,
        CancellationToken cancellationToken
    )
    {
        return ComponentGeneralizationAuthorization.IsAuthorizedToManage(
            claimsPrincipal,
            _association.GeneralComponentId,
            _association.ConcreteComponentId,
            userManager,
            context,
            cancellationToken
        );
    }
}