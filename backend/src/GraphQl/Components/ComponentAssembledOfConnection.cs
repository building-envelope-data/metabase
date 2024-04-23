using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using Metabase.Authorization;
using Metabase.GraphQl.Users;
using Microsoft.AspNetCore.Identity;

namespace Metabase.GraphQl.Components
{
    public sealed class ComponentAssembledOfConnection
        : Connection<Data.Component, Data.ComponentAssembly, ComponentPartsByComponentIdDataLoader,
            ComponentAssembledOfEdge>
    {
        public ComponentAssembledOfConnection(
            Data.Component subject
        )
            : base(
                subject,
                x => new ComponentAssembledOfEdge(x)
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
            return ComponentAssemblyAuthorization.IsAuthorizedToAdd(
                claimsPrincipal,
                Subject.Id,
                userManager,
                context,
                cancellationToken
            );
        }
    }
}