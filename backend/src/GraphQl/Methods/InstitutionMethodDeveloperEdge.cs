using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using Metabase.Authorization;
using Metabase.GraphQl.Institutions;
using Metabase.GraphQl.Users;
using Microsoft.AspNetCore.Identity;

namespace Metabase.GraphQl.Methods;

public sealed class InstitutionMethodDeveloperEdge
    : Edge<Data.Institution, InstitutionByIdDataLoader>
{
    private readonly Data.InstitutionMethodDeveloper _association;

    public InstitutionMethodDeveloperEdge(
        Data.InstitutionMethodDeveloper association
    )
        : base(association.InstitutionId)
    {
        _association = association;
    }

    [UseUserManager]
    public Task<bool> CanCurrentUserConfirmEdgeAsync(
        ClaimsPrincipal claimsPrincipal,
        [Service(ServiceKind.Resolver)] UserManager<Data.User> userManager,
        Data.ApplicationDbContext context,
        CancellationToken cancellationToken
    )
    {
        return InstitutionMethodDeveloperAuthorization.IsAuthorizedToConfirm(
            claimsPrincipal,
            _association.InstitutionId,
            userManager,
            context,
            cancellationToken
        );
    }

    [UseUserManager]
    public Task<bool> CanCurrentUserRemoveEdgeAsync(
        ClaimsPrincipal claimsPrincipal,
        [Service(ServiceKind.Resolver)] UserManager<Data.User> userManager,
        Data.ApplicationDbContext context,
        CancellationToken cancellationToken
    )
    {
        return InstitutionMethodDeveloperAuthorization.IsAuthorizedToRemove(
            claimsPrincipal,
            _association.MethodId,
            userManager,
            context,
            cancellationToken
        );
    }
}