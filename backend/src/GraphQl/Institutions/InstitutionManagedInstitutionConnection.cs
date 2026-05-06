using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut.Data;
using HotChocolate.CostAnalysis.Types;
using Metabase.Authorization;
using Metabase.Data;
using Metabase.GraphQl.Users;

namespace Metabase.GraphQl.Institutions;

public sealed class InstitutionManagedInstitutionConnection(
    Institution institution,
    PagingArguments pagingArguments,
    QueryContext<Institution> queryContext
)
: PaginatedConnection<Institution, Institution, InstitutionManagedInstitutionEdge, IInstitutionManagedInstitutionsByInstitutionIdDataLoader>(
        institution,
        (node, cursor) => new InstitutionManagedInstitutionEdge(node, cursor),
        pagingArguments,
        queryContext
)
{
    [UseUserManager]
    [Cost(1)]
    public Task<bool> IsAuthorizedToAddEdgeAsync(
        ClaimsPrincipal claimsPrincipal,
        InstitutionAuthorization authorization,
        CancellationToken cancellationToken
    )
    {
        return authorization.IsAuthorizedToCreateInstitutionManagedByInstitution(
            claimsPrincipal,
            Subject.Id,
            cancellationToken
        );
    }
}