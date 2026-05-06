using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut.Data;
using HotChocolate.CostAnalysis.Types;
using Metabase.Data;
using Metabase.Data.OpenIdConnect;
using Metabase.GraphQl.Users;

namespace Metabase.GraphQl.Institutions;

public sealed class InstitutionOwnedOpenIdConnectApplicationConnection(
    Institution institution,
    QueryContext<OpenIdConnectApplication> queryContext
) : AuthorizedConnection<
        Institution,
        OpenIdConnectApplication,
        InstitutionOwnedOpenIdConnectApplicationEdge,
        InstitutionOwnedOpenIdConnectApplicationsByInstitutionIdDataLoader,
        Authorization.OpenIdConnectAuthorization
    >
(
    institution,
    x => new InstitutionOwnedOpenIdConnectApplicationEdge(x),
    (claimsPrincipal, institution, authorization, cancellationToken) =>
        authorization.IsAuthorizedToManageApplications(claimsPrincipal, institution.Id, cancellationToken),
    queryContext
)
{
    [UseUserManager]
    [Cost(1)]
    public Task<bool> IsAuthorizedToAddEdgeAsync(
        ClaimsPrincipal claimsPrincipal,
        Authorization.OpenIdConnectAuthorization authorization,
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
    [Cost(1)]
    public Task<bool> IsAuthorizedToRemoveEdgeAsync(
        ClaimsPrincipal claimsPrincipal,
        Authorization.OpenIdConnectAuthorization authorization,
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