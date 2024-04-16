using System.Diagnostics.CodeAnalysis;

namespace Metabase.GraphQl.Users
{
    [SuppressMessage("Naming", "CA1707")]
    public enum ConfirmUserEmailErrorCode
    {
        UNKNOWN,
        UNKNOWN_USER,
        INVALID_CONFIRMATION_CODE
    }
}