using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Metabase.Authorization;
using Metabase.Data;
using Metabase.GraphQl.Institutions;
using Metabase.GraphQl.Users;
using Microsoft.AspNetCore.Identity;

namespace Metabase.GraphQl.Components;

public sealed class ComponentManufacturerEdge(
    ComponentManufacturer association
    )
        : Edge<Institution, InstitutionByIdDataLoader>(association.InstitutionId)
{
    private readonly ComponentManufacturer _association = association;

    [UseUserManager]
    public Task<bool> IsAuthorizedToConfirmEdgeAsync(
        ClaimsPrincipal claimsPrincipal,
        ComponentManufacturerAuthorization authorization,
        CancellationToken cancellationToken
    )
    {
        return authorization.IsAuthorizedToConfirm(
            claimsPrincipal,
            _association.InstitutionId,
            cancellationToken
        );
    }

    [UseUserManager]
    public Task<bool> IsAuthorizedToRemoveEdgeAsync(
        ClaimsPrincipal claimsPrincipal,
        ComponentManufacturerAuthorization authorization,
        CancellationToken cancellationToken
    )
    {
        return authorization.IsAuthorizedToRemove(
            claimsPrincipal,
            _association.InstitutionId,
            cancellationToken
        );
    }
}