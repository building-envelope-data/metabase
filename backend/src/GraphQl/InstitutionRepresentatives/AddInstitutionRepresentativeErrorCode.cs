using System.Diagnostics.CodeAnalysis;

namespace Metabase.GraphQl.InstitutionRepresentatives
{
    [SuppressMessage("Naming", "CA1707")]
    public enum AddInstitutionRepresentativeErrorCode
    {
        UNKNOWN,
        UNKNOWN_INSTITUTION,
        UNKNOWN_USER,
        DUPLICATE,
        UNAUTHORIZED
    }
}