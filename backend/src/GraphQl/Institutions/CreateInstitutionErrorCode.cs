using System.Diagnostics.CodeAnalysis;

namespace Metabase.GraphQl.Institutions
{
    [SuppressMessage("Naming", "CA1707")]
    public enum CreateInstitutionErrorCode
    {
        UNKNOWN,
        NEITHER_OWNER_NOR_MANAGER,
        UNKNOWN_OWNERS,
        UNKNOWN_MANAGER,
        UNAUTHORIZED
    }
}