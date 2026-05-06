using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut.Data;
using HotChocolate.CostAnalysis.Types;
using Metabase.Authorization;
using Metabase.Data;
using Metabase.GraphQl.Users;

namespace Metabase.GraphQl.Institutions;

public sealed class InstitutionManagedComponentConnection(
    Institution institution,
    PagingArguments pagingArguments,
    QueryContext<Component> queryContext
    )
        : PaginatedConnection<Institution, Component, InstitutionManagedComponentEdge, IInstitutionManagedComponentsByInstitutionIdDataLoader>(
        institution,
        (node, cursor) => new InstitutionManagedComponentEdge(node, cursor),
        pagingArguments,
        queryContext
        )
{
    [UseUserManager]
    [Cost(1)]
    public Task<bool> IsAuthorizedToAddEdgeAsync(
        ClaimsPrincipal claimsPrincipal,
        ComponentAuthorization authorization,
        CancellationToken cancellationToken
    )
    {
        return authorization.IsAuthorizedToCreateComponentForInstitution(
            claimsPrincipal,
            Subject.Id,
            cancellationToken
        );
    }
}