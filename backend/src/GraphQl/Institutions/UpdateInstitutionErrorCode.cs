using System.Diagnostics.CodeAnalysis;

namespace Metabase.GraphQl.Institutions
{
    [SuppressMessage("Naming", "CA1707")]
    public enum UpdateInstitutionErrorCode
    {
        UNKNOWN,
        UNAUTHORIZED,
        UNKNOWN_INSTITUTION
    }
}