using System.Diagnostics.CodeAnalysis;

namespace Metabase.GraphQl.InstitutionRepresentatives;

[SuppressMessage("Naming", "CA1707")]
public enum RemoveInstitutionRepresentativeErrorCode
{
    UNKNOWN,
    LAST_OWNER,
    UNAUTHORIZED,
    UNKNOWN_INSTITUTION,
    UNKNOWN_REPRESENTATIVE,
    UNKNOWN_USER
}