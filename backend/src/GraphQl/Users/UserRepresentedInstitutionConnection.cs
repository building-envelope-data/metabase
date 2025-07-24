using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Metabase.Authorization;
using Metabase.Data;

namespace Metabase.GraphQl.Users;

public sealed class UserRepresentedInstitutionConnection(
    User subject,
    bool pending
    )
        : ForkingConnection<User, InstitutionRepresentative,
        PendingUserRepresentedInstitutionsByUserIdDataLoader, UserRepresentedInstitutionsByUserIdDataLoader,
        UserRepresentedInstitutionEdge>(
        subject,
        pending,
        x => new UserRepresentedInstitutionEdge(x)
        )
{
    [UseUserManager]
    public Task<bool> CanCurrentUserConfirmEdgeAsync(
        ClaimsPrincipal claimsPrincipal,
        InstitutionRepresentativeAuthorization authorization,
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