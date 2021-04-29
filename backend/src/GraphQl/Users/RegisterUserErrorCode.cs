namespace Metabase.GraphQl.Users
{
    public enum RegisterUserErrorCode
    {
        UNKNOWN,
        PASSWORD_CONFIRMATION_MISMATCH,
        DUPLICATE_EMAIL,
        INVALID_EMAIL,
        PASSWORD_REQUIRES_DIGIT,
        PASSWORD_REQUIRES_LOWER,
        PASSWORD_REQUIRES_NON_ALPHANUMERIC,
        PASSWORD_REQUIRES_UPPER,
        PASSWORD_TOO_SHORT,
        NULL_OR_EMPTY_EMAIL
    }
}