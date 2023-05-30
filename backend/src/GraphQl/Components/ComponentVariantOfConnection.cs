using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using Metabase.Authorization;
using Metabase.GraphQl.Users;
using Microsoft.AspNetCore.Identity;

namespace Metabase.GraphQl.Components
{
    public sealed class ComponentVariantOfConnection
        : Connection<Data.Component, Data.ComponentVariant, ComponentVariantOfByComponentIdDataLoader, ComponentVariantOfEdge>
    {
        public ComponentVariantOfConnection(
            Data.Component subject
        )
            : base(
                subject,
                x => new ComponentVariantOfEdge(x)
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
            return ComponentVariantAuthorization.IsAuthorizedToAdd(
                 claimsPrincipal,
                 Subject.Id,
                 userManager,
                 context,
                 cancellationToken
                 );
        }
    }
}