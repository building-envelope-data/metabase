namespace Metabase.GraphQl.Users
{
    public enum LoginUserWithRecoveryCodeErrorCode
    {
        UNKNOWN,
        UNKNOWN_USER,
        NOT_ALLOWED,
        LOCKED_OUT,
        INVALID_RECOVERY_CODE
    }
}