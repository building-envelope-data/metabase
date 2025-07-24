using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Metabase.Data;
using Metabase.Data.OpenIdConnect;
using Metabase.GraphQl.OpenIdConnect.Application;
using Metabase.GraphQl.Users;

namespace Metabase.GraphQl.Institutions;

public sealed class InstitutionOpenIdConnectApplicationEdge(
    InstitutionOpenIdConnectApplication association
) : Edge<OpenIdConnectApplication, OpenIdConnectApplicationByIdDataLoader>
(
    association.ApplicationId
)
{
    private readonly InstitutionOpenIdConnectApplication _association = association;

    [UseUserManager]
    public Task<bool> CanCurrentUserRemoveEdgeAsync(
        ClaimsPrincipal claimsPrincipal,
        Authorization.OpenIdConnectAuthorization authorization,
        CancellationToken cancellationToken
    )
    {
        return authorization.IsAuthorizedToManageApplication(
            claimsPrincipal,
            _association.ApplicationId,
            cancellationToken
        );
    }
}