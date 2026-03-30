using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut.Data;
using HotChocolate.CostAnalysis.Types;
using Metabase.Authorization;
using Metabase.Data;
using Metabase.GraphQl.Users;

namespace Metabase.GraphQl.Institutions;

public sealed class InstitutionManufacturedComponentConnection(
    Institution subject,
    PagingArguments pagingArguments,
    QueryContext<ComponentManufacturer> queryContext
)
: PaginatedConnection<Institution, ComponentManufacturer, InstitutionManufacturedComponentEdge, IInstitutionManufacturedComponentsByInstitutionIdDataLoader>(
    subject,
    (association, cursor) => new InstitutionManufacturedComponentEdge(association, cursor),
    pagingArguments,
    queryContext
)
{
}

public sealed class PendingInstitutionManufacturedComponentConnection(
    Institution subject,
    PagingArguments pagingArguments,
    QueryContext<ComponentManufacturer> queryContext
)
: AuthorizedPaginatedConnection<Institution, ComponentManufacturer, InstitutionManufacturedComponentEdge, IPendingInstitutionManufacturedComponentsByInstitutionIdDataLoader, ComponentManufacturerAuthorization>(
    subject,
    (association, cursor) => new InstitutionManufacturedComponentEdge(association, cursor),
    (claimsPrincipal, authorization, cancellationToken) =>
        authorization.IsAuthorizedToConfirm(claimsPrincipal, subject.Id, cancellationToken),
    pagingArguments,
    queryContext
)
{
    [UseUserManager]
    [Cost(1)]
    public Task<bool> IsAuthorizedToConfirmEdgesAsync(
        ClaimsPrincipal claimsPrincipal,
        ComponentManufacturerAuthorization authorization,
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