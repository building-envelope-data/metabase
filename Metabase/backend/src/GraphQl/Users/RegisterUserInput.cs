namespace Metabase.GraphQl.Users
{
  public record RegisterUserInput(
        string Email,
        string Password,
        string PasswordConfirmation
      );
}
