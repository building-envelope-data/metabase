using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Data;
using Metabase.Authorization;
using Metabase.GraphQl.Users;
using Microsoft.AspNetCore.Identity;

namespace Metabase.GraphQl.Institutions
{
    public sealed class InstitutionManufacturedComponentConnection
        : ForkingConnection<Data.Institution, Data.ComponentManufacturer, PendingInstitutionManufacturedComponentsByInstitutionIdDataLoader, InstitutionManufacturedComponentsByInstitutionIdDataLoader, InstitutionManufacturedComponentEdge>
    {
        public InstitutionManufacturedComponentConnection(
            Data.Institution institution,
            bool pending
        )
            : base(
                institution,
                pending,
                x => new InstitutionManufacturedComponentEdge(x)
                )
        {
        }

        [UseDbContext(typeof(Data.ApplicationDbContext))]
        [UseUserManager]
        public Task<bool> CanCurrentUserAddEdgeAsync(
            [GlobalState(nameof(ClaimsPrincipal))] ClaimsPrincipal claimsPrincipal,
            [ScopedService] UserManager<Data.User> userManager,
            [ScopedService] Data.ApplicationDbContext context,
            CancellationToken cancellationToken
        )
        {
            return ComponentAuthorization.IsAuthorizedToCreateComponentForInstitution(
                 claimsPrincipal,
                 Subject.Id,
                 userManager,
                 context,
                 cancellationToken
                 );
        }
    }
}