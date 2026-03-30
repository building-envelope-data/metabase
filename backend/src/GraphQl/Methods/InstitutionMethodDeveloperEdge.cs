using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate.CostAnalysis.Types;
using Metabase.Authorization;
using Metabase.Data;
using Metabase.GraphQl.Institutions;
using Metabase.GraphQl.Users;

namespace Metabase.GraphQl.Methods;

public sealed class InstitutionMethodDeveloperEdge(
    InstitutionMethodDeveloper association
    )
        : Edge<Institution, IInstitutionByIdDataLoader>(association.InstitutionId)
{
    [UseUserManager]
    [Cost(1)]
    public Task<bool> IsAuthorizedToConfirmEdgeAsync(
        ClaimsPrincipal claimsPrincipal,
        InstitutionMethodDeveloperAuthorization authorization,
        CancellationToken cancellationToken
    )
    {
        return authorization.IsAuthorizedToConfirm(
            claimsPrincipal,
            association.InstitutionId,
            cancellationToken
        );
    }

    [UseUserManager]
    [Cost(1)]
    public Task<bool> IsAuthorizedToRemoveEdgeAsync(
        ClaimsPrincipal claimsPrincipal,
        InstitutionMethodDeveloperAuthorization authorization,
        CancellationToken cancellationToken
    )
    {
        return authorization.IsAuthorizedToRemove(
            claimsPrincipal,
            association.MethodId,
            cancellationToken
        );
    }
}