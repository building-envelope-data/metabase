using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using Metabase.Authorization;
using Metabase.GraphQl.Users;
using Microsoft.AspNetCore.Identity;

namespace Metabase.GraphQl.Institutions
{
    public sealed class InstitutionRepresentativeConnection
        : ForkingConnection<Data.Institution, Data.InstitutionRepresentative, PendingInstitutionRepresentativesByInstitutionIdDataLoader, InstitutionRepresentativesByInstitutionIdDataLoader, InstitutionRepresentativeEdge>
    {
        public InstitutionRepresentativeConnection(
            Data.Institution institution,
            bool pending
        )
            : base(
                institution,
                pending,
                x => new InstitutionRepresentativeEdge(x)
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
            return InstitutionRepresentativeAuthorization.IsAuthorizedToManage(
                 claimsPrincipal,
                 Subject.Id,
                 userManager,
                 context,
                 cancellationToken
                 );
        }
    }
}