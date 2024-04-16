using System.Diagnostics.CodeAnalysis;

namespace Metabase.GraphQl.Users
{
    [SuppressMessage("Naming", "CA1707")]
    public enum SetUserPhoneNumberErrorCode
    {
        UNKNOWN,
        UNKNOWN_USER,
        UNCHANGED_PHONE_NUMBER
    }
}