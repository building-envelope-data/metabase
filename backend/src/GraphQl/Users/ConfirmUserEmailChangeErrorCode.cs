using System.Diagnostics.CodeAnalysis;

namespace Metabase.GraphQl.Users
{
    [SuppressMessage("Naming", "CA1707")]
    public enum ConfirmUserEmailChangeErrorCode
    {
        UNKNOWN,
        DUPLICATE_EMAIL,
        INVALID_CONFIRMATION_CODE,
        UNKNOWN_USER
    }
}