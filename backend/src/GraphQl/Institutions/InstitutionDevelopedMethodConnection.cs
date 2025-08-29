using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut.Data;
using Metabase.Authorization;
using Metabase.Data;
using Metabase.GraphQl.Users;

namespace Metabase.GraphQl.Institutions;

public sealed class InstitutionDevelopedMethodConnection(
    Institution institution,
    bool pending,
    QueryContext<InstitutionMethodDeveloper> queryContext
    )
        : ForkingConnection<Institution, InstitutionMethodDeveloper,
        PendingInstitutionDevelopedMethodsByInstitutionIdDataLoader,
        InstitutionDevelopedMethodsByInstitutionIdDataLoader, InstitutionDevelopedMethodEdge>(
        institution,
        pending,
        x => new InstitutionDevelopedMethodEdge(x),
        queryContext
        )
{
    [UseUserManager]
    public Task<bool> CanCurrentUserConfirmEdgeAsync(
        ClaimsPrincipal claimsPrincipal,
        InstitutionMethodDeveloperAuthorization authorization,
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