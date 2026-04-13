using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut.Data;
using Metabase.Authorization;
using Metabase.Data;
using Metabase.GraphQl.Users;
using Microsoft.AspNetCore.Identity;

namespace Metabase.GraphQl.Institutions;

public sealed class InstitutionManagedMethodConnection(
    Institution institution,
    QueryContext<Method> queryContext
    )
        : Connection<Institution, Method, InstitutionManagedMethodsByInstitutionIdDataLoader,
        InstitutionManagedMethodEdge>(
        institution,
        x => new InstitutionManagedMethodEdge(x),
        queryContext
        )
{
    [UseUserManager]
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