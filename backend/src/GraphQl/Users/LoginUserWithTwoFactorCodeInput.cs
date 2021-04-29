namespace Metabase.GraphQl.Users
{
    public record LoginUserWithTwoFactorCodeInput(
          string AuthenticatorCode,
          bool RememberMe,
          bool RememberMachine
        );
}