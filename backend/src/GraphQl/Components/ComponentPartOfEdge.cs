using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using Metabase.Authorization;
using Metabase.GraphQl.Users;
using Microsoft.AspNetCore.Identity;

namespace Metabase.GraphQl.Components
{
    public sealed class ComponentPartOfEdge
        : Edge<Data.Component, ComponentByIdDataLoader>
    {
        private readonly Data.ComponentAssembly _association;

        public ComponentPartOfEdge(
            Data.ComponentAssembly association
        )
            : base(association.AssembledComponentId)
        {
            _association = association;
        }

        [UseUserManager]
        public Task<bool> CanCurrentUserRemoveEdgeAsync(
            [GlobalState(nameof(ClaimsPrincipal))] ClaimsPrincipal claimsPrincipal,
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
}