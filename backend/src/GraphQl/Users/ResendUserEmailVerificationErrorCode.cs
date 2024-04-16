using System.Diagnostics.CodeAnalysis;

namespace Metabase.GraphQl.Users
{
    [SuppressMessage("Naming", "CA1707")]
    public enum ResendUserEmailVerificationErrorCode
    {
        UNKNOWN,
        UNKNOWN_USER,
        UNKNOWN_EMAIL
    }
}