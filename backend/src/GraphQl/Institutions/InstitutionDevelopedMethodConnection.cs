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
    QueryContext<InstitutionMethodDeveloper> queryContext
    )
        : Connection<Institution, InstitutionMethodDeveloper, InstitutionDevelopedMethodsByInstitutionIdDataLoader, InstitutionDevelopedMethodEdge>(
        institution,
        x => new InstitutionDevelopedMethodEdge(x),
        queryContext
        )
{
    [UseUserManager]
    public Task<bool> IsAuthorizedToConfirmEdgeAsync(
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

public sealed class PendingInstitutionDevelopedMethodConnection(
    Institution institution,
    QueryContext<InstitutionMethodDeveloper> queryContext
    )
        : AuthorizedConnection<Institution, InstitutionMethodDeveloper, PendingInstitutionDevelopedMethodsByInstitutionIdDataLoader, InstitutionDevelopedMethodEdge, InstitutionMethodDeveloperAuthorization>(
        institution,
        x => new InstitutionDevelopedMethodEdge(x),
        (claimsPrincipal, institution, authorization, cancellationToken) =>
            authorization.IsAuthorizedToConfirm(claimsPrincipal, institution.Id, cancellationToken),
        queryContext
        )
{
    [UseUserManager]
    public Task<bool> IsAuthorizedToConfirmEdgeAsync(
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