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
    public sealed class InstitutionManagedInstitutionConnection
        : Connection<Data.Institution, Data.Institution, InstitutionManagedInstitutionsByInstitutionIdDataLoader, InstitutionManagedInstitutionEdge>
    {
        public InstitutionManagedInstitutionConnection(
            Data.Institution institution
        )
            : base(
                institution,
                x => new InstitutionManagedInstitutionEdge(x)
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
            return InstitutionAuthorization.IsAuthorizedToCreateInstitutionManagedByInstitution(
                 claimsPrincipal,
                 Subject.Id,
                 userManager,
                 context,
                 cancellationToken
                 );
        }
    }
}