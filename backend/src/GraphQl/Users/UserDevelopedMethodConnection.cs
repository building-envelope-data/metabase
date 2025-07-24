using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Metabase.Authorization;
using Metabase.Data;

namespace Metabase.GraphQl.Users;

public sealed class UserDevelopedMethodConnection(
    User subject,
    bool pending
    )
        : ForkingConnection<User, UserMethodDeveloper, PendingUserDevelopedMethodsByUserIdDataLoader,
        UserDevelopedMethodsByUserIdDataLoader, UserDevelopedMethodEdge>(
        subject,
        pending,
        x => new UserDevelopedMethodEdge(x)
        )
{
    [UseUserManager]
    public Task<bool> CanCurrentUserConfirmEdgeAsync(
        ClaimsPrincipal claimsPrincipal,
        UserMethodDeveloperAuthorization authorization,
        CancellationToken cancellationToken
    )
    {
        return authorization.IsAuthorizedToConfirm(
            claimsPrincipal,
            Subject.Id,
            cancellationToken
        );
    }
}