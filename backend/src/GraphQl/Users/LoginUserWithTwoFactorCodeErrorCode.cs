namespace Metabase.GraphQl.Users
{
    public enum LoginUserWithTwoFactorCodeErrorCode
    {
        UNKNOWN,
        UNKNOWN_USER,
        NOT_ALLOWED,
        LOCKED_OUT,
        INVALID_AUTHENTICATOR_CODE
    }
}