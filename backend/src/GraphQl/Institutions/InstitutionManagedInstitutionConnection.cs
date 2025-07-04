using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Metabase.Authorization;
using Metabase.Data;
using Metabase.GraphQl.Users;
using Microsoft.AspNetCore.Identity;

namespace Metabase.GraphQl.Institutions;

public sealed class InstitutionManagedInstitutionConnection(
    Institution institution
    )
        : Connection<Institution, Institution, InstitutionManagedInstitutionsByInstitutionIdDataLoader,
        InstitutionManagedInstitutionEdge>(
        institution,
        x => new InstitutionManagedInstitutionEdge(x)
        )
{
    [UseUserManager]
    public Task<bool> CanCurrentUserAddEdgeAsync(
        ClaimsPrincipal claimsPrincipal,
        InstitutionAuthorization authorization,
        CancellationToken cancellationToken
    )
    {
        return authorization.IsAuthorizedToCreateInstitutionManagedByInstitution(
            claimsPrincipal,
            Subject.Id,
            cancellationToken
        );
    }
}