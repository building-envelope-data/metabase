namespace Metabase.GraphQl.Users
{
    public record SetUserPasswordInput(
          string Password,
          string PasswordConfirmation
        );
}