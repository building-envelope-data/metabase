using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut.Data;
using Metabase.Authorization;
using Metabase.Data;
using Metabase.GraphQl.Users;

namespace Metabase.GraphQl.Institutions;

public sealed class InstitutionRepresentativeConnection(
    Institution institution,
    bool pending,
    QueryContext<InstitutionRepresentative> queryContext
    )
        : ForkingConnection<Institution, InstitutionRepresentative,
        PendingInstitutionRepresentativesByInstitutionIdDataLoader,
        InstitutionRepresentativesByInstitutionIdDataLoader, InstitutionRepresentativeEdge>(
        institution,
        pending,
        x => new InstitutionRepresentativeEdge(x),
        queryContext
        )
{
    [UseUserManager]
    public Task<bool> CanCurrentUserAddEdgeAsync(
        ClaimsPrincipal claimsPrincipal,
        InstitutionRepresentativeAuthorization authorization,
        CancellationToken cancellationToken
    )
    {
        return authorization.IsAuthorizedToManage(
            claimsPrincipal,
            Subject.Id,
            cancellationToken
        );
    }
}