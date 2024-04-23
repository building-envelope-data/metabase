namespace Metabase.GraphQl.Users
{
    public sealed record LoginUserWithTwoFactorCodeInput(
        string AuthenticatorCode,
        bool RememberMachine
    );
}