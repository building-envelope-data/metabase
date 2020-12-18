namespace Metabase.GraphQl.Users
{
  public record ResetUserPasswordInput(
        string Email,
        string Password,
        string PasswordConfirmation,
        string ResetCode
      );
}
