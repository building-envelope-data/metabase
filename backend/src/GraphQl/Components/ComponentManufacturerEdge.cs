using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using Metabase.Authorization;
using Metabase.GraphQl.Users;
using Microsoft.AspNetCore.Identity;
using Metabase.GraphQl.Institutions;

namespace Metabase.GraphQl.Components
{
    public sealed class ComponentManufacturerEdge
        : Edge<Data.Institution, InstitutionByIdDataLoader>
    {
        private readonly Data.ComponentManufacturer _association;
        public ComponentManufacturerEdge(
            Data.ComponentManufacturer association
        )
            : base(association.InstitutionId)
        {
            _association = association;
        }

        [UseUserManager]
        public Task<bool> CanCurrentUserConfirmEdgeAsync(
            ClaimsPrincipal claimsPrincipal,
            [Service(ServiceKind.Resolver)] UserManager<Data.User> userManager,
            Data.ApplicationDbContext context,
            CancellationToken cancellationToken
        )
        {
            return ComponentManufacturerAuthorization.IsAuthorizedToConfirm(
                 claimsPrincipal,
                 _association.InstitutionId,
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
            return ComponentManufacturerAuthorization.IsAuthorizedToRemove(
                 claimsPrincipal,
                 _association.InstitutionId,
                 userManager,
                 context,
                 cancellationToken
                 );
        }
    }
}