using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut.Data;
using Metabase.Authorization;
using Metabase.Data;
using Metabase.GraphQl.Users;

namespace Metabase.GraphQl.Institutions;

public sealed class InstitutionOperatedDatabaseConnection(
    Institution institution,
    QueryContext<Database> queryContext
    )
        : Connection<Institution, Database, InstitutionOperatedDatabasesByInstitutionIdDataLoader,
        InstitutionOperatedDatabaseEdge>(
        institution,
        x => new InstitutionOperatedDatabaseEdge(x),
        queryContext
        )
{
    [UseUserManager]
    public Task<bool> IsAuthorizedToAddEdgeAsync(
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