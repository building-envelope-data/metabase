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
    public sealed class InstitutionManagedDataFormatConnection
        : Connection<Data.Institution, Data.DataFormat, InstitutionManagedDataFormatsByInstitutionIdDataLoader, InstitutionManagedDataFormatEdge>
    {
        public InstitutionManagedDataFormatConnection(
            Data.Institution institution
        )
            : base(
                institution,
                x => new InstitutionManagedDataFormatEdge(x)
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
            return DataFormatAuthorization.IsAuthorizedToCreateDataFormatForInstitution(
                 claimsPrincipal,
                 Subject.Id,
                 userManager,
                 context,
                 cancellationToken
                 );
        }
    }
}