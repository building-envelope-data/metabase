using System.Diagnostics.CodeAnalysis;

namespace Metabase.GraphQl.Institutions;

[SuppressMessage("Naming", "CA1707")]
public enum SetInstitutionExtrasErrorCode
{
    UNKNOWN,
    UNAUTHORIZED,
    UNKNOWN_INSTITUTION
}