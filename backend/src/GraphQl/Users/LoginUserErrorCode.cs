using System.Diagnostics.CodeAnalysis;

namespace Metabase.GraphQl.Users
{
    [SuppressMessage("Naming", "CA1707")]
    public enum LoginUserErrorCode
    {
        INVALID,
        LOCKED_OUT,
        NOT_ALLOWED,
        UNKNOWN
    }
}