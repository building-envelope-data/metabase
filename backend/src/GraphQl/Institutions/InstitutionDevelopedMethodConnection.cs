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
    public sealed class InstitutionDevelopedMethodConnection
        : ForkingConnection<Data.Institution, Data.InstitutionMethodDeveloper, PendingInstitutionDevelopedMethodsByInstitutionIdDataLoader, InstitutionDevelopedMethodsByInstitutionIdDataLoader, InstitutionDevelopedMethodEdge>
    {
        public InstitutionDevelopedMethodConnection(
            Data.Institution institution,
            bool pending
        )
            : base(
                institution,
                pending,
                x => new InstitutionDevelopedMethodEdge(x)
                )
        {
        }

        [UseDbContext(typeof(Data.ApplicationDbContext))]
        [UseUserManager]
        public Task<bool> CanCurrentUserConfirmEdgeAsync(
            [GlobalState(nameof(ClaimsPrincipal))] ClaimsPrincipal claimsPrincipal,
            [ScopedService] UserManager<Data.User> userManager,
            [ScopedService] Data.ApplicationDbContext context,
            CancellationToken cancellationToken
        )
        {
            return InstitutionMethodDeveloperAuthorization.IsAuthorizedToConfirm(
                 claimsPrincipal,
                 Subject.Id,
                 userManager,
                 context,
                 cancellationToken
                 );
        }
    }
}