using System.Diagnostics.CodeAnalysis;

namespace Metabase.GraphQl.Users
{
    [SuppressMessage("Naming", "CA1707")]
    public enum GenerateUserTwoFactorRecoveryCodesErrorCode
    {
        UNKNOWN,
        UNKNOWN_USER,
        TWO_FACTOR_AUTHENTICATION_DISABLED,
        CODE_GENERATION_FAILED
    }
}