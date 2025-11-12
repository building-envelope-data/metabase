using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut.Data;
using Metabase.Authorization;
using Metabase.Data;
using Metabase.GraphQl.Users;
using Microsoft.AspNetCore.Identity;

namespace Metabase.GraphQl.Institutions;

public sealed class InstitutionManagedDataFormatConnection(
    Institution institution,
    QueryContext<DataFormat> queryContext
    )
        : Connection<Institution, DataFormat, InstitutionManagedDataFormatsByInstitutionIdDataLoader,
        InstitutionManagedDataFormatEdge>(
        institution,
        x => new InstitutionManagedDataFormatEdge(x),
        queryContext
        )
{
    [UseUserManager]
    public Task<bool> IsAuthorizedToAddEdgeAsync(
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