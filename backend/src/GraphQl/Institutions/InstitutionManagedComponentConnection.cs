using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut.Data;
using Metabase.Authorization;
using Metabase.Data;
using Metabase.GraphQl.Users;
using Microsoft.AspNetCore.Identity;

namespace Metabase.GraphQl.Institutions;

public sealed class InstitutionManagedComponentConnection(
    Institution institution,
    QueryContext<Component> queryContext
    )
        : Connection<Institution, Component, InstitutionManagedComponentsByInstitutionIdDataLoader,
        InstitutionManagedComponentEdge>(
        institution,
        x => new InstitutionManagedComponentEdge(x),
        queryContext
        )
{
    [UseUserManager]
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