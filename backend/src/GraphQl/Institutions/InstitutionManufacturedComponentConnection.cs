using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Metabase.Authorization;
using Metabase.Data;
using Metabase.GraphQl.Users;
using Microsoft.AspNetCore.Identity;

namespace Metabase.GraphQl.Institutions;

public sealed class InstitutionManufacturedComponentConnection(
    Institution institution,
    bool pending
    )
        : ForkingConnection<Institution, ComponentManufacturer,
        PendingInstitutionManufacturedComponentsByInstitutionIdDataLoader,
        InstitutionManufacturedComponentsByInstitutionIdDataLoader, InstitutionManufacturedComponentEdge>(
        institution,
        pending,
        x => new InstitutionManufacturedComponentEdge(x)
        )
{
    [UseUserManager]
    public Task<bool> CanCurrentUserAddEdgeAsync(
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

    [UseUserManager]
    public Task<bool> CanCurrentUserConfirmEdgeAsync(
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