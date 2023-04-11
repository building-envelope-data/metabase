using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using Metabase.Authorization;
using Metabase.GraphQl.Users;
using Microsoft.AspNetCore.Identity;

namespace Metabase.GraphQl.Components
{
    public sealed class ComponentManufacturerConnection
        : ForkingConnection<Data.Component, Data.ComponentManufacturer, PendingComponentManufacturersByComponentIdDataLoader, ComponentManufacturersByComponentIdDataLoader, ComponentManufacturerEdge>
    {
        public ComponentManufacturerConnection(
            Data.Component subject,
            bool pending
        )
            : base(
                subject,
                pending,
                x => new ComponentManufacturerEdge(x)
                )
        {
        }

        [UseUserManager]
        public Task<bool> CanCurrentUserAddEdgeAsync(
            [GlobalState(nameof(ClaimsPrincipal))] ClaimsPrincipal claimsPrincipal,
            [Service(ServiceKind.Resolver)] UserManager<Data.User> userManager,
            Data.ApplicationDbContext context,
            CancellationToken cancellationToken
        )
        {
            return ComponentManufacturerAuthorization.IsAuthorizedToAdd(
                 claimsPrincipal,
                 Subject.Id,
                 userManager,
                 context,
                 cancellationToken
                 );
        }
    }
}