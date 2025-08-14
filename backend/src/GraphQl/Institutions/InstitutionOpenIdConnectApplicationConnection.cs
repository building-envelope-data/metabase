using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut.Data;
using Metabase.Authorization;
using Metabase.Data;
using Metabase.GraphQl.Users;

namespace Metabase.GraphQl.Institutions;

public sealed class InstitutionOpenIdConnectApplicationConnection(
    Institution institution,
    QueryContext<InstitutionOpenIdConnectApplication> queryContext
) : AuthorizedConnection<
        Institution,
        InstitutionOpenIdConnectApplication,
        InstitutionOpenIdConnectApplicationsByInstitutionIdDataLoader,
        InstitutionOpenIdConnectApplicationEdge,
        OpenIdConnectAuthorization
    >
(
    institution,
    x => new InstitutionOpenIdConnectApplicationEdge(x),
    (claimsPrincipal, institution, authorization, cancellationToken) =>
        authorization.IsAuthorizedToManageApplications(claimsPrincipal, institution.Id, cancellationToken),
    queryContext
)
{
    [UseUserManager]
    public Task<bool> CanCurrentUserAddEdgeAsync(
        ClaimsPrincipal claimsPrincipal,
        OpenIdConnectAuthorization authorization,
        CancellationToken cancellationToken
    )
    {
        return authorization.IsAuthorizedToManageApplications(
            claimsPrincipal,
            Subject.Id,
            cancellationToken
        );
    }

    [UseUserManager]
    public Task<bool> CanCurrentUserRemoveEdgeAsync(
        ClaimsPrincipal claimsPrincipal,
        OpenIdConnectAuthorization authorization,
        CancellationToken cancellationToken
    )
    {
        return authorization.IsAuthorizedToManageApplications(
            claimsPrincipal,
            Subject.Id,
            cancellationToken
        );
    }
}