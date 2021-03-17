namespace Metabase.GraphQl.Users
{
    public record ChangeUserPasswordInput(
          string CurrentPassword,
          string NewPassword,
          string NewPasswordConfirmation
        );
}