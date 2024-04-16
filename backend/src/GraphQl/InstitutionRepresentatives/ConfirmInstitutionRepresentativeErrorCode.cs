using System.Diagnostics.CodeAnalysis;

namespace Metabase.GraphQl.InstitutionRepresentatives
{
    [SuppressMessage("Naming", "CA1707")]
    public enum ConfirmInstitutionRepresentativeErrorCode
    {
        UNKNOWN,
        UNKNOWN_INSTITUTION,
        UNKNOWN_USER,
        UNAUTHORIZED,
        UNKNOWN_REPRESENTATIVE,
    }
}