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
    public sealed class InstitutionRepresentativeConnection
        : Connection<Data.Institution, Data.InstitutionRepresentative, InstitutionRepresentativesByInstitutionIdDataLoader, InstitutionRepresentativeEdge>
    {
        public InstitutionRepresentativeConnection(
            Data.Institution institution
        )
            : base(
                institution,
                x => new InstitutionRepresentativeEdge(x)
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
            return InstitutionAuthorization.IsAuthorizedToManageRepresentatives(
                 claimsPrincipal,
                 Subject.Id,
                 userManager,
                 context,
                 cancellationToken
                 );
        }
    }
}