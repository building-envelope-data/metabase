using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate.CostAnalysis.Types;
using Metabase.Authorization;
using Metabase.Data;
using Metabase.Enumerations;
using Metabase.GraphQl.Institutions;

namespace Metabase.GraphQl.Users;

public sealed class UserRepresentedInstitutionEdge(
    InstitutionRepresentative association
    )
        : Edge<Institution, IInstitutionByIdDataLoader>(association.InstitutionId)
{
    public InstitutionRepresentativeRole Role { get; } = association.Role;

    [UseUserManager]
    [Cost(1)]
    public Task<bool> IsAuthorizedToRemoveEdgeAsync(
        ClaimsPrincipal claimsPrincipal,
        InstitutionRepresentativeAuthorization authorization,
        CancellationToken cancellationToken
    )
    {
        return authorization.IsAuthorizedToManage(
            claimsPrincipal,
            association.InstitutionId,
            cancellationToken
        );
    }
}