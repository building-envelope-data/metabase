using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut.Data;
using Metabase.Authorization;
using Metabase.Data;
using Metabase.GraphQl.Users;

namespace Metabase.GraphQl.Institutions;

public sealed class InstitutionGnuPgKeyFingerprintConnection(
    Institution institution,
    QueryContext<GnuPgKeyFingerprint> queryContext
) : Connection<
        Institution,
        GnuPgKeyFingerprint,
        GnuPgKeyFingerprintsByInstitutionIdDataLoader,
        InstitutionGnuPgKeyFingerprintEdge
    >
(
    institution,
    x => new InstitutionGnuPgKeyFingerprintEdge(x),
    queryContext
)
{
    [UseUserManager]
    public Task<bool> CanCurrentUserAddEdgeAsync(
        ClaimsPrincipal claimsPrincipal,
        GnuPgKeyFingerprintAuthorization authorization,
        CancellationToken cancellationToken
    )
    {
        return authorization.IsAuthorizedToAdd(
            claimsPrincipal,
            Subject.Id,
            cancellationToken
        );
    }
}