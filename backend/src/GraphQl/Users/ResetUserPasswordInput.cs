namespace Metabase.GraphQl.Users
{
    public sealed record ResetUserPasswordInput(
          string Email,
          string Password,
          string PasswordConfirmation,
          string ResetCode
        );
}