using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Metabase.Authorization;
using Metabase.Data;
using Metabase.Enumerations;
using Metabase.GraphQl.Users;

namespace Metabase.GraphQl.Institutions;

public sealed class InstitutionRepresentativeEdge(
    InstitutionRepresentative association
)
: Edge<User, UserByIdDataLoader>(association.UserId)
{
    public InstitutionRepresentativeRole Role { get; } = association.Role;

    [UseUserManager]
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