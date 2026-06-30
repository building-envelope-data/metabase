using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut.Data;
using HotChocolate.CostAnalysis.Types;
using Metabase.Authorization;
using Metabase.Data;

namespace Metabase.GraphQl.Users;

public sealed class UserDevelopedMethodConnection(
    User subject,
    PagingArguments pagingArguments,
    QueryContext<UserMethodDeveloper> queryContext
    )
        : PaginatedConnection<User, UserMethodDeveloper, UserDevelopedMethodEdge, IUserDevelopedMethodsByUserIdDataLoader>(
        subject,
        (association, cursor) => new UserDevelopedMethodEdge(association, cursor),
        pagingArguments,
        queryContext
        )
{
}

public sealed class PendingUserDevelopedMethodConnection(
    User subject,
    PagingArguments pagingArguments,
    QueryContext<UserMethodDeveloper> queryContext
    )
        : AuthorizedPaginatedConnection<User, UserMethodDeveloper, UserDevelopedMethodEdge, IPendingUserDevelopedMethodsByUserIdDataLoader, UserMethodDeveloperAuthorization>(
        subject,
        (association, cursor) => new UserDevelopedMethodEdge(association, cursor),
        (claimsPrincipal, authorization, cancellationToken) =>
            authorization.IsAuthorizedToConfirm(claimsPrincipal, subject.Id, cancellationToken),
        pagingArguments,
        queryContext
        )
{
    [UseUserManager]
    [Cost(1)]
    public Task<bool> IsAuthorizedToConfirmEdgesAsync(
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