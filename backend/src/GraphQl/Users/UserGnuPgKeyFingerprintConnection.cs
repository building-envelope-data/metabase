using GreenDonut.Data;
using Metabase.Data;

namespace Metabase.GraphQl.Users;

public sealed class UserGnuPgKeyFingerprintConnection(
    User user,
    QueryContext<GnuPgKeyFingerprint> queryContext
) : Connection<
        User,
        GnuPgKeyFingerprint,
        GnuPgKeyFingerprintsByUserIdDataLoader,
        UserGnuPgKeyFingerprintEdge
    >
(
    user,
    x => new UserGnuPgKeyFingerprintEdge(x),
    queryContext
)
{
}