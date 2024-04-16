using System.Diagnostics.CodeAnalysis;

namespace Metabase.GraphQl.InstitutionMethodDevelopers
{
    [SuppressMessage("Naming", "CA1707")]
    public enum AddInstitutionMethodDeveloperErrorCode
    {
        UNKNOWN,
        UNKNOWN_METHOD,
        UNKNOWN_INSTITUTION,
        UNAUTHORIZED,
        DUPLICATE
    }
}