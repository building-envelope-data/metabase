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
    public sealed class InstitutionManagedMethodConnection
        : Connection<Data.Institution, Data.Method, InstitutionManagedMethodsByInstitutionIdDataLoader, InstitutionManagedMethodEdge>
    {
        public InstitutionManagedMethodConnection(
            Data.Institution institution
        )
            : base(
                institution,
                x => new InstitutionManagedMethodEdge(x)
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
            return MethodAuthorization.IsAuthorizedToCreateMethodManagedByInstitution(
                 claimsPrincipal,
                 Subject.Id,
                 userManager,
                 context,
                 cancellationToken
                 );
        }
    }
}