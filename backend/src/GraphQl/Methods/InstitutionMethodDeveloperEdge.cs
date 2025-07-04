using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Metabase.Authorization;
using Metabase.Data;
using Metabase.GraphQl.Institutions;
using Metabase.GraphQl.Users;
using Microsoft.AspNetCore.Identity;

namespace Metabase.GraphQl.Methods;

public sealed class InstitutionMethodDeveloperEdge(
    InstitutionMethodDeveloper association
    )
        : Edge<Institution, InstitutionByIdDataLoader>(association.InstitutionId)
{
    private readonly InstitutionMethodDeveloper _association = association;

    [UseUserManager]
    public Task<bool> CanCurrentUserConfirmEdgeAsync(
        ClaimsPrincipal claimsPrincipal,
        InstitutionMethodDeveloperAuthorization authorization,
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
    public Task<bool> CanCurrentUserRemoveEdgeAsync(
        ClaimsPrincipal claimsPrincipal,
        InstitutionMethodDeveloperAuthorization authorization,
        CancellationToken cancellationToken
    )
    {
        return authorization.IsAuthorizedToRemove(
            claimsPrincipal,
            _association.MethodId,
            cancellationToken
        );
    }
}