namespace Metabase.GraphQl.Users
{
    public record LoginUserInput(
          string Email,
          string Password,
          bool RememberMe
        );
}