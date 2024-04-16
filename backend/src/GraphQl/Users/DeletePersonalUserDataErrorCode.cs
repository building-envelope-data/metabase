using System.Diagnostics.CodeAnalysis;

namespace Metabase.GraphQl.Users
{
    [SuppressMessage("Naming", "CA1707")]
    public enum DeletePersonalUserDataErrorCode
    {
        UNKNOWN,
        UNKNOWN_USER,
        MISSING_PASSWORD,
        INCORRECT_PASSWORD
    }
}