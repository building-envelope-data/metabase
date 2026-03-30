using Metabase.Data;
using Metabase.GraphQl.Users;

namespace Metabase.GraphQl.GnuPgKeyFingerprints;

public sealed class GnuPgKeyFingerprintUserEdge(
    GnuPgKeyFingerprint association
    )
        : Edge<User, IUserByIdDataLoader>(association.UserId)
{
}