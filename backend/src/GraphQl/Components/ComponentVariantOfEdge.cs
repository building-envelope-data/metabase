using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using Metabase.Authorization;
using Metabase.GraphQl.Users;
using Microsoft.AspNetCore.Identity;

namespace Metabase.GraphQl.Components
{
    public sealed class ComponentVariantOfEdge
        : Edge<Data.Component, ComponentByIdDataLoader>
    {
        private readonly Data.ComponentVariant _association;

        public ComponentVariantOfEdge(
            Data.ComponentVariant association
        )
            : base(association.OfComponentId)
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
            return ComponentAssemblyAuthorization.IsAuthorizedToManage(
                 claimsPrincipal,
                 _association.OfComponentId,
                 _association.ToComponentId,
                 userManager,
                 context,
                 cancellationToken
                 );
        }
    }
}