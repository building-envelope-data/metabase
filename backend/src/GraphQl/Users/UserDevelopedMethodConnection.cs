using System.Security.Claims;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Data;
using Metabase.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Metabase.GraphQl.Users
{
    public sealed class UserDevelopedMethodConnection
        : ForkingConnection<Data.User, Data.UserMethodDeveloper, PendingUserDevelopedMethodsByUserIdDataLoader, UserDevelopedMethodsByUserIdDataLoader, UserDevelopedMethodEdge>
    {
        public UserDevelopedMethodConnection(
            Data.User subject,
            bool pending
        )
            : base(
                subject,
                pending,
                x => new UserDevelopedMethodEdge(x)
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
            return UserMethodDeveloperAuthorization.IsAuthorizedToConfirm(
                 claimsPrincipal,
                 Subject.Id,
                 userManager
                 );
        }
    }
}