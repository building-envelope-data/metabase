using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut.Data;
using Metabase.Authorization;
using Metabase.Data;
using Metabase.GraphQl.Users;
using Microsoft.AspNetCore.Identity;

namespace Metabase.GraphQl.Institutions;

public sealed class InstitutionManufacturedComponentConnection(
    Institution institution,
    QueryContext<ComponentManufacturer> queryContext
    )
        : Connection<Institution, ComponentManufacturer, InstitutionManufacturedComponentsByInstitutionIdDataLoader, InstitutionManufacturedComponentEdge>(
        institution,
        x => new InstitutionManufacturedComponentEdge(x),
        queryContext
        )
{
    [UseUserManager]
    public Task<bool> IsAuthorizedToConfirmEdgeAsync(
        ClaimsPrincipal claimsPrincipal,
        ComponentManufacturerAuthorization authorization,
        CancellationToken cancellationToken
    )
    {
        return authorization.IsAuthorizedToConfirm(
            claimsPrincipal,
            Subject.Id,
            cancellationToken
        );
    }
}

public sealed class PendingInstitutionManufacturedComponentConnection(
    Institution institution,
    QueryContext<ComponentManufacturer> queryContext
    )
        : AuthorizedConnection<Institution, ComponentManufacturer, PendingInstitutionManufacturedComponentsByInstitutionIdDataLoader, InstitutionManufacturedComponentEdge, ComponentManufacturerAuthorization>(
        institution,
        x => new InstitutionManufacturedComponentEdge(x),
        (claimsPrincipal, institution, authorization, cancellationToken) =>
            authorization.IsAuthorizedToConfirm(claimsPrincipal, institution.Id, cancellationToken),
        queryContext
        )
{
    [UseUserManager]
    public Task<bool> IsAuthorizedToConfirmEdgeAsync(
        ClaimsPrincipal claimsPrincipal,
        ComponentManufacturerAuthorization authorization,
        CancellationToken cancellationToken
    )
    {
        return authorization.IsAuthorizedToConfirm(
            claimsPrincipal,
            Subject.Id,
            cancellationToken
        );
    }
}