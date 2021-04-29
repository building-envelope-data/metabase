namespace Metabase.GraphQl.Users
{
    public record RegisterUserInput(
          string Name,
          string Email,
          string Password,
          string PasswordConfirmation
        );
}