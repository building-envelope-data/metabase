using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Metabase.Authorization;
using Metabase.Data;
using Metabase.GraphQl.Users;
using Microsoft.AspNetCore.Identity;

namespace Metabase.GraphQl.Institutions;

public sealed class InstitutionManagedDataFormatConnection(
    Institution institution
    )
        : Connection<Institution, DataFormat, InstitutionManagedDataFormatsByInstitutionIdDataLoader,
        InstitutionManagedDataFormatEdge>(
        institution,
        x => new InstitutionManagedDataFormatEdge(x)
        )
{
    [UseUserManager]
    public Task<bool> CanCurrentUserAddEdgeAsync(
        ClaimsPrincipal claimsPrincipal,
        DataFormatAuthorization authorization,
        CancellationToken cancellationToken
    )
    {
        return authorization.IsAuthorizedToCreateDataFormatForInstitution(
            claimsPrincipal,
            Subject.Id,
            cancellationToken
        );
    }
}