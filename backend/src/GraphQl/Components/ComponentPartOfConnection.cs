using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using Metabase.Authorization;
using Metabase.GraphQl.Users;
using Microsoft.AspNetCore.Identity;

namespace Metabase.GraphQl.Components
{
    public sealed class ComponentPartOfConnection
        : Connection<Data.Component, Data.ComponentAssembly, ComponentPartOfByComponentIdDataLoader, ComponentPartOfEdge>
    {
        public ComponentPartOfConnection(
            Data.Component subject
        )
            : base(
                subject,
                x => new ComponentPartOfEdge(x)
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