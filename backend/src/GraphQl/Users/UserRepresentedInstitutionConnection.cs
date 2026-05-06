using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut.Data;
using HotChocolate.CostAnalysis.Types;
using Metabase.Authorization;
using Metabase.Data;

namespace Metabase.GraphQl.Users;

public sealed class UserRepresentedInstitutionConnection(
    User subject,
    QueryContext<InstitutionRepresentative> queryContext
)
: Connection<
    User,
    InstitutionRepresentative,
    UserRepresentedInstitutionEdge,
    UserRepresentedInstitutionsByUserIdDataLoader
>(
    subject,
    x => new UserRepresentedInstitutionEdge(x),
    queryContext
    )
{
}

public sealed class PendingUserRepresentedInstitutionConnection(
    User subject,
    QueryContext<InstitutionRepresentative> queryContext
)
: AuthorizedConnection<
    User,
    InstitutionRepresentative,
    UserRepresentedInstitutionEdge,
    PendingUserRepresentedInstitutionsByUserIdDataLoader,
    InstitutionRepresentativeAuthorization
>(
    subject,
    x => new UserRepresentedInstitutionEdge(x),
    (claimsPrincipal, user, authorization, cancellationToken) =>
        authorization.IsAuthorizedToConfirm(claimsPrincipal, user.Id, cancellationToken),
    queryContext
    )
{
    [UseUserManager]
    [Cost(1)]
    public Task<bool> IsAuthorizedToConfirmEdgesAsync(
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