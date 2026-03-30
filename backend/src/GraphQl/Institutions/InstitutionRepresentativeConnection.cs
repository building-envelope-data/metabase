using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut.Data;
using HotChocolate.CostAnalysis.Types;
using Metabase.Authorization;
using Metabase.Data;
using Metabase.GraphQl.Users;

namespace Metabase.GraphQl.Institutions;

public sealed class InstitutionRepresentativeConnection(
    Institution institution,
    QueryContext<InstitutionRepresentative> queryContext
    )
        : Connection<Institution, InstitutionRepresentative, InstitutionRepresentativeEdge, IInstitutionRepresentativesByInstitutionIdDataLoader>(
        institution,
        x => new InstitutionRepresentativeEdge(x),
        queryContext
        )
{
    [UseUserManager]
    [Cost(1)]
    public Task<bool> IsAuthorizedToAddEdgeAsync(
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

public sealed class PendingInstitutionRepresentativeConnection(
    Institution institution,
    QueryContext<InstitutionRepresentative> queryContext
    )
        : AuthorizedConnection<Institution, InstitutionRepresentative, InstitutionRepresentativeEdge, IPendingInstitutionRepresentativesByInstitutionIdDataLoader, InstitutionRepresentativeAuthorization>(
        institution,
        x => new InstitutionRepresentativeEdge(x),
        (claimsPrincipal, institution, authorization, cancellationToken) =>
            authorization.IsAuthorizedToManage(claimsPrincipal, institution.Id, cancellationToken),
        queryContext
        )
{
    [UseUserManager]
    [Cost(1)]
    public Task<bool> IsAuthorizedToAddEdgeAsync(
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