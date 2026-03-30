using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut.Data;
using HotChocolate.CostAnalysis.Types;
using Metabase.Authorization;
using Metabase.Data;
using Metabase.GraphQl.Users;

namespace Metabase.GraphQl.Institutions;

public sealed class InstitutionDevelopedMethodConnection(
    Institution institution,
    PagingArguments pagingArguments,
    QueryContext<InstitutionMethodDeveloper> queryContext
    )
        : PaginatedConnection<Institution, InstitutionMethodDeveloper, InstitutionDevelopedMethodEdge, IInstitutionDevelopedMethodsByInstitutionIdDataLoader>(
        institution,
        (association, cursor) => new InstitutionDevelopedMethodEdge(association, cursor),
        pagingArguments,
        queryContext
        )
{
}

public sealed class PendingInstitutionDevelopedMethodConnection(
    Institution institution,
    PagingArguments pagingArguments,
    QueryContext<InstitutionMethodDeveloper> queryContext
    )
        : AuthorizedPaginatedConnection<Institution, InstitutionMethodDeveloper, InstitutionDevelopedMethodEdge, IPendingInstitutionDevelopedMethodsByInstitutionIdDataLoader, InstitutionMethodDeveloperAuthorization>(
        institution,
        (association, cursor) => new InstitutionDevelopedMethodEdge(association, cursor),
        (claimsPrincipal, authorization, cancellationToken) =>
            authorization.IsAuthorizedToConfirm(claimsPrincipal, institution.Id, cancellationToken),
        pagingArguments,
        queryContext
        )
{
    [UseUserManager]
    [Cost(1)]
    public Task<bool> IsAuthorizedToConfirmEdgesAsync(
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