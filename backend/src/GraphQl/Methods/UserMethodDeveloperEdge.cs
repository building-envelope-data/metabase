using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using Metabase.Authorization;
using Metabase.GraphQl.Users;
using Microsoft.AspNetCore.Identity;


namespace Metabase.GraphQl.Methods
{
    public sealed class UserMethodDeveloperEdge
        : Edge<Data.User, UserByIdDataLoader>
    {
        private readonly Data.UserMethodDeveloper _association;

        public UserMethodDeveloperEdge(
            Data.UserMethodDeveloper association
        )
            : base(association.UserId)
        {
            _association = association;
        }

        [UseUserManager]
        public Task<bool> CanCurrentUserConfirmEdgeAsync(
            [GlobalState(nameof(ClaimsPrincipal))] ClaimsPrincipal claimsPrincipal,
            [Service(ServiceKind.Resolver)] UserManager<Data.User> userManager
        )
        {
            return UserMethodDeveloperAuthorization.IsAuthorizedToConfirm(
                 claimsPrincipal,
                 _association.UserId,
                 userManager
                 );
        }

        [UseUserManager]
        public Task<bool> CanCurrentUserRemoveEdgeAsync(
            [GlobalState(nameof(ClaimsPrincipal))] ClaimsPrincipal claimsPrincipal,
            [Service(ServiceKind.Resolver)] UserManager<Data.User> userManager,
            Data.ApplicationDbContext context,
            CancellationToken cancellationToken
        )
        {
            return UserMethodDeveloperAuthorization.IsAuthorizedToRemove(
                 claimsPrincipal,
                 _association.MethodId,
                 userManager,
                 context,
                 cancellationToken
                 );
        }
    }
}