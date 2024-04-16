using System.Diagnostics.CodeAnalysis;

namespace Metabase.GraphQl.Users
{
    [SuppressMessage("Naming", "CA1707")]
    public enum ChangeUserEmailErrorCode
    {
        UNKNOWN,
        UNKNOWN_USER,
        UNCHANGED_EMAIL,
        UNKNOWN_CURRENT_EMAIL
    }
}