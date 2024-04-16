using System.Diagnostics.CodeAnalysis;

namespace Metabase.GraphQl.Users
{
    [SuppressMessage("Naming", "CA1707")]
    public enum DeleteUserErrorCode
    {
        UNKNOWN,
        UNKNOWN_USER,
        UNAUTHORIZED,
    }
}