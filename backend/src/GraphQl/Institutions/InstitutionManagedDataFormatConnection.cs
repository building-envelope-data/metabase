using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut.Data;
using HotChocolate.CostAnalysis.Types;
using Metabase.Authorization;
using Metabase.Data;
using Metabase.GraphQl.Users;

namespace Metabase.GraphQl.Institutions;

public sealed class InstitutionManagedDataFormatConnection(
    Institution institution,
    PagingArguments pagingArguments,
    QueryContext<DataFormat> queryContext
    )
        : PaginatedConnection<Institution, DataFormat, InstitutionManagedDataFormatEdge, IInstitutionManagedDataFormatsByInstitutionIdDataLoader>(
        institution,
        (node, cursor) => new InstitutionManagedDataFormatEdge(node, cursor),
        pagingArguments,
        queryContext
        )
{
    [UseUserManager]
    [Cost(1)]
    public Task<bool> IsAuthorizedToAddEdgeAsync(
        ClaimsPrincipal claimsPrincipal,
        DataFormatAuthorization authorization,
        CancellationToken cancellationToken
    )
    {
        return authorization.IsAuthorizedToCreateDataFormatForInstitution(
            claimsPrincipal,
            Subject.Id,
            cancellationToken
        );
    }
}