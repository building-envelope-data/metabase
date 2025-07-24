using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Metabase.Authorization;
using Metabase.Data;
using Metabase.GraphQl.Users;

namespace Metabase.GraphQl.Institutions;

public sealed class InstitutionOpenIdConnectApplicationConnection(
    Institution institution
) : Connection<
        Institution,
        InstitutionOpenIdConnectApplication,
        InstitutionOpenIdConnectApplicationsByInstitutionIdDataLoader,
        InstitutionOpenIdConnectApplicationEdge
    >
(
    institution,
    x => new InstitutionOpenIdConnectApplicationEdge(x)
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