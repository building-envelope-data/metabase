using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using Metabase.Authorization;
using Metabase.GraphQl.Users;
using Microsoft.AspNetCore.Identity;

namespace Metabase.GraphQl.Components
{
    public sealed class ComponentConcretizationOfConnection
        : Connection<Data.Component, Data.ComponentConcretizationAndGeneralization, ComponentGeneralizationsByComponentIdDataLoader, ComponentConcretizationOfEdge>
    {
        public ComponentConcretizationOfConnection(
            Data.Component subject
        )
            : base(
                subject,
                x => new ComponentConcretizationOfEdge(x)
                )
        {
        }

        [UseUserManager]
        public Task<bool> CanCurrentUserAddEdgeAsync(
            ClaimsPrincipal claimsPrincipal,
            [Service(ServiceKind.Resolver)] UserManager<Data.User> userManager,
            Data.ApplicationDbContext context,
            CancellationToken cancellationToken
        )
        {
            return ComponentGeneralizationAuthorization.IsAuthorizedToAdd(
                 claimsPrincipal,
                 Subject.Id,
                 userManager,
                 context,
                 cancellationToken
                 );
        }
    }
}