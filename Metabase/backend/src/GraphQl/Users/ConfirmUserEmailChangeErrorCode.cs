namespace Metabase.GraphQl.Users
{
    public enum ConfirmUserEmailChangeErrorCode
    {
      UNKNOWN,
      DUPLICATE_EMAIL,
      INVALID_CONFIRMATION_TOKEN,
      USER_NOT_FOUND
    }
}
