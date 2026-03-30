using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut.Data;
using HotChocolate.CostAnalysis.Types;
using Metabase.Authorization;
using Metabase.Data;
using Metabase.GraphQl.Users;

namespace Metabase.GraphQl.Institutions;

public sealed class InstitutionManagedMethodConnection(
    Institution institution,
    PagingArguments pagingArguments,
    QueryContext<Method> queryContext
)
: PaginatedConnection<Institution, Method, InstitutionManagedMethodEdge, IInstitutionManagedMethodsByInstitutionIdDataLoader>(
        institution,
        (node, cursor) => new InstitutionManagedMethodEdge(node, cursor),
        pagingArguments,
        queryContext
        )
{
    [UseUserManager]
    [Cost(1)]
    public Task<bool> IsAuthorizedToAddEdgeAsync(
        ClaimsPrincipal claimsPrincipal,
        MethodAuthorization authorization,
        CancellationToken cancellationToken
    )
    {
        return authorization.IsAuthorizedToCreateMethodManagedByInstitution(
            claimsPrincipal,
            Subject.Id,
            cancellationToken
        );
    }
}