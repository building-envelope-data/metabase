using Metabase.Data;
using Metabase.GraphQl.Institutions;

namespace Metabase.GraphQl.GnuPgKeyFingerprints;

public sealed class GnuPgKeyFingerprintInstitutionEdge(
    GnuPgKeyFingerprint association
    )
        : Edge<Institution, InstitutionByIdDataLoader>(association.InstitutionId)
{
}