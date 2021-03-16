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
    public sealed class InstitutionOperatedDatabaseConnection
        : Connection<Data.Institution, Data.Database, InstitutionOperatedDatabasesByInstitutionIdDataLoader, InstitutionOperatedDatabaseEdge>
    {
        public InstitutionOperatedDatabaseConnection(
            Data.Institution institution
        )
            : base(
                institution,
                x => new InstitutionOperatedDatabaseEdge(x)
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
            return DatabaseAuthorization.IsAuthorizedToCreateDatabaseForInstitution(
                 claimsPrincipal,
                 Subject.Id,
                 userManager,
                 context,
                 cancellationToken
                 );
        }
    }
}