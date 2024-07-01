using System.Diagnostics.CodeAnalysis;

namespace Metabase.GraphQl.Institutions;

[SuppressMessage("Naming", "CA1707")]
public enum SwitchInstitutionOperatingStateErrorCode
{
    UNKNOWN,
    UNKNOWN_INSTITUTION,
    UNAUTHORIZED
}