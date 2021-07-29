using System.Security.Claims;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Data;
using Metabase.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Metabase.GraphQl.Users
{
    public sealed class UserRepresentedInstitutionConnection
        : ForkingConnection<Data.User, Data.InstitutionRepresentative, PendingUserRepresentedInstitutionsByUserIdDataLoader, UserRepresentedInstitutionsByUserIdDataLoader, UserRepresentedInstitutionEdge>
    {
        public UserRepresentedInstitutionConnection(
            Data.User subject,
            bool pending
        )
            : base(
                subject,
                pending,
                x => new UserRepresentedInstitutionEdge(x)
                )
        {
        }

        [UseDbContext(typeof(Data.ApplicationDbContext))]
        [UseUserManager]
        public Task<bool> CanCurrentUserConfirmEdgeAsync(
            [GlobalState(nameof(ClaimsPrincipal))] ClaimsPrincipal claimsPrincipal,
            [ScopedService] UserManager<Data.User> userManager
        )
        {
            return InstitutionRepresentativeAuthorization.IsAuthorizedToConfirm(
                 claimsPrincipal,
                 Subject.Id,
                 userManager
                 );
        }
    }
}