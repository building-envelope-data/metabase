using System.Diagnostics.CodeAnalysis;

namespace Metabase.GraphQl.InstitutionRepresentatives;

[SuppressMessage("Naming", "CA1707")]
public enum GrantPermissionToSignDataErrorCode
{
    UNKNOWN,
    UNKNOWN_USER,
    UNKNOWN_INSTITUTION,
    UNAUTHORIZED
}