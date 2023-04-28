namespace Metabase.GraphQl.Users
{
    public sealed record LoginUserWithTwoFactorCodeInput(
          string AuthenticatorCode,
          bool RememberMe,
          bool RememberMachine
        );
}