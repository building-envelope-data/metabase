using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Metabase.Authorization;
using Metabase.Data;
using Metabase.GraphQl.Users;
using Microsoft.AspNetCore.Identity;

namespace Metabase.GraphQl.Institutions;

public sealed class InstitutionOperatedDatabaseConnection(
    Institution institution
    )
        : Connection<Institution, Database, InstitutionOperatedDatabasesByInstitutionIdDataLoader,
        InstitutionOperatedDatabaseEdge>(
        institution,
        x => new InstitutionOperatedDatabaseEdge(x)
        )
{
    [UseUserManager]
    public Task<bool> CanCurrentUserAddEdgeAsync(
        ClaimsPrincipal claimsPrincipal,
        DatabaseAuthorization authorization,
        CancellationToken cancellationToken
    )
    {
        return authorization.IsAuthorizedToCreateDatabaseForInstitution(
            claimsPrincipal,
            Subject.Id,
            cancellationToken
        );
    }
}