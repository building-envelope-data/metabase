using System.Diagnostics.CodeAnalysis;

namespace Metabase.GraphQl.InstitutionMethodDevelopers
{
    [SuppressMessage("Naming", "CA1707")]
    public enum RemoveInstitutionMethodDeveloperErrorCode
    {
        UNKNOWN,
        UNKNOWN_METHOD,
        UNKNOWN_INSTITUTION,
        UNAUTHORIZED,
        UNKNOWN_DEVELOPER,
    }
}