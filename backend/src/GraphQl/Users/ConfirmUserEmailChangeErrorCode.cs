namespace Metabase.GraphQl.Users
{
    public enum ConfirmUserEmailChangeErrorCode
    {
        UNKNOWN,
        DUPLICATE_EMAIL,
        INVALID_CONFIRMATION_CODE,
        UNKNOWN_USER
    }
}