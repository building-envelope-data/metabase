using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut.Data;
using Metabase.Authorization;
using Metabase.Data;

namespace Metabase.GraphQl.Users;

public sealed class UserDevelopedMethodConnection(
    User subject,
    QueryContext<UserMethodDeveloper> queryContext
    )
        : Connection<User, UserMethodDeveloper, UserDevelopedMethodsByUserIdDataLoader, UserDevelopedMethodEdge>(
        subject,
        x => new UserDevelopedMethodEdge(x),
        queryContext
        )
{
    [UseUserManager]
    public Task<bool> IsAuthorizedToConfirmEdgeAsync(
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

public sealed class PendingUserDevelopedMethodConnection(
    User subject,
    QueryContext<UserMethodDeveloper> queryContext
    )
        : AuthorizedConnection<User, UserMethodDeveloper, PendingUserDevelopedMethodsByUserIdDataLoader, UserDevelopedMethodEdge, UserMethodDeveloperAuthorization>(
        subject,
        x => new UserDevelopedMethodEdge(x),
        (claimsPrincipal, institution, authorization, cancellationToken) =>
            authorization.IsAuthorizedToConfirm(claimsPrincipal, institution.Id, cancellationToken),
        queryContext
        )
{
    [UseUserManager]
    public Task<bool> IsAuthorizedToConfirmEdgeAsync(
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