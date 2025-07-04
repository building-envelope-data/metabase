using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Metabase.Authorization;
using Metabase.Data;
using Metabase.GraphQl.Users;
using Microsoft.AspNetCore.Identity;

namespace Metabase.GraphQl.Methods;

public sealed class UserMethodDeveloperEdge(
    UserMethodDeveloper association
    )
        : Edge<User, UserByIdDataLoader>(association.UserId)
{
    private readonly UserMethodDeveloper _association = association;

    [UseUserManager]
    public Task<bool> CanCurrentUserConfirmEdgeAsync(
        ClaimsPrincipal claimsPrincipal,
        UserMethodDeveloperAuthorization authorization
    )
    {
        return authorization.IsAuthorizedToConfirm(
            claimsPrincipal,
            _association.UserId
        );
    }

    [UseUserManager]
    public Task<bool> CanCurrentUserRemoveEdgeAsync(
        ClaimsPrincipal claimsPrincipal,
        UserMethodDeveloperAuthorization authorization,
        CancellationToken cancellationToken
    )
    {
        return authorization.IsAuthorizedToRemove(
            claimsPrincipal,
            _association.MethodId,
            cancellationToken
        );
    }
}